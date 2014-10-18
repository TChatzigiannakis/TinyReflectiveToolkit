/*
 *  Tiny Reflective Toolkit
    Copyright (C) 2014  Theodoros Chatzigiannakis

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>. 
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Reflection.Emit;
using System.Security.Policy;

namespace TinyReflectiveToolkit
{
    /// <summary>
    /// Extension methods enabling post-build interface implementation.
    /// </summary>
    public static class RuntimeContractExtensions
    {
        private static readonly AssemblyBuilder DynamicAssembly =
            Thread.GetDomain()
                .DefineDynamicAssembly(new AssemblyName("TinyReflectiveToolkit-Dynamic"),
                    AssemblyBuilderAccess.RunAndSave);

        private const string DynamicAssemblyName = "Dynamic.dll";
        private const string ActualObjectFieldName = "InternalObject";
        private const string ProxyNamespace = "TinyReflectiveToolkit.Contracts";

        private static readonly ModuleBuilder ModuleBuilder =
            DynamicAssembly.DefineDynamicModule("Contracts", DynamicAssemblyName);

        private static readonly Dictionary<Tuple<Type, Type>, Type> ContractToProxyDictionary = 
            new Dictionary<Tuple<Type, Type>, Type>();

        /// <summary>
        /// Returns a runtime-generated proxy that implements a specified interface and forwards method calls to the given object - even if the relationship between the object's type and the interface wasn't declared at build time.
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TContract ToContract<TContract>(this object obj)
            where TContract : class
        {
            return CreateContractProxyFromObject<TContract>(obj);
        }

        private static TContract CreateContractProxyFromObject<TContract>(object actualObject, bool saveAssemblyForDebuggingPurposes = false)
            where TContract : class
        {
            // Check arguments.
            var contract = typeof (TContract);
            if (!contract.IsInterface) throw new NotSupportedException("TContract must be an interface type.");
            if (!contract.IsPublic) throw new NotSupportedException(contract.Name + " must be public.");

            var actualObjectType = actualObject.GetType();
            if (!actualObjectType.IsPublic) throw new NotSupportedException(actualObjectType.Name + " must be public.");

            // Reuse an existing proxy type if possible.
            var typeContractCombination = new Tuple<Type, Type>(actualObjectType, contract);
            if (ContractToProxyDictionary.ContainsKey(typeContractCombination))
                return actualObject.GenerateProxy<TContract>(typeContractCombination);
            
            // Start building a new proxy type.
            var guid = Guid.NewGuid().ToString();
            var sanitizedGuid = guid.Replace("-", "");
            var proxyName = ProxyNamespace + "." + contract.Name + "_" + sanitizedGuid;
            var proxyBuilder = ModuleBuilder.DefineType(proxyName, TypeAttributes.Public, null, new[] {contract});
            var fieldWithActualObject = proxyBuilder.DefineField(ActualObjectFieldName, actualObjectType, FieldAttributes.Public);

            // Read contract.
            var methodsToImplement = contract.GetMethods()
                .WithoutAttribute<ExplicitConversionAttribute>()
                .WithoutAttribute<ImplicitConversionAttribute>()
                .ToList();
            var explicitConversionsToImplement = contract.GetMethods()
                .WithAttribute<ExplicitConversionAttribute>()
                .ToList();
            var implicitConversionsToImplement = contract.GetMethods()
                .WithAttribute<ImplicitConversionAttribute>()
                .ToList();

            // Find regular method implementations.
            var actualMethodsForThisObject = methodsToImplement.Select(x =>
            {
                var name = x.Name;
                var parameters = x.GetParameters().Select(n => n.ParameterType).ToArray();
                return actualObjectType.GetMethod(name, parameters);
            }).ToList();
            
            // Find implementations for operators.
            var mimicObjectOperators = actualObjectType.GetMethods()
                .Where(m => m.IsStatic)
                .Where(m => m.Name.StartsWith("op_"))
                .ToList();
            var actualExplicitConversionsForThisObject = explicitConversionsToImplement.Select(x =>
            {
                var conversions = mimicObjectOperators
                    .Where(m => m.ReturnType == x.ReturnType)
                    .Where(m => m.Name == "op_Explicit")
                    .ToList();
                return new {Name = x.Name, CastFunction = conversions.Single()};
            }).ToList();
            var actualImplicitConversionsForThisObject = implicitConversionsToImplement.Select(x =>
            {
                var conversions = mimicObjectOperators
                    .Where(m => m.ReturnType == x.ReturnType)
                    .Where(m => m.Name == "op_Implicit")
                    .ToList();
                return new {Name = x.Name, CastFunction = conversions.Single()};
            }).ToList();

            // Implement regular methods.
            const MethodAttributes proxyMethodAttributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot |
                                                           MethodAttributes.Final; 
            var proxyImplementations = actualMethodsForThisObject.Select(x =>
            {
                var name = x.Name;
                var parameters = x.GetParameters().Select(n => n.ParameterType).ToArray();
                var retType = x.ReturnType;
                var proxyMethod = proxyBuilder.DefineMethod(name, proxyMethodAttributes, retType, parameters);
                var generator = proxyMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, fieldWithActualObject);
                for (var i = 0; i < parameters.Count(); i++)
                    generator.Emit(OpCodes.Ldarg, i + 1);
                generator.EmitCall(OpCodes.Callvirt, x, null);
                generator.Emit(OpCodes.Ret);
                return proxyMethod;
            }).ToList();


            // Implement operators.
            var explicitConversionsInProxy = actualExplicitConversionsForThisObject.Select(x =>
            {
                var name = x.Name;
                var retType = x.CastFunction.ReturnType;
                var proxyMethod = proxyBuilder.DefineMethod(name, proxyMethodAttributes, retType, new Type[0]);
                var generator = proxyMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, fieldWithActualObject);
                generator.EmitCall(OpCodes.Call, x.CastFunction, null);
                generator.Emit(OpCodes.Ret);
                return proxyMethod;
            }).ToList();
            var implicitConversionsInProxy = actualImplicitConversionsForThisObject.Select(x =>
            {
                var name = x.Name;
                var retType = x.CastFunction.ReturnType;
                var proxyMethod = proxyBuilder.DefineMethod(name, proxyMethodAttributes, retType, new Type[0]);
                var generator = proxyMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, fieldWithActualObject);
                generator.EmitCall(OpCodes.Call, x.CastFunction, null);
                generator.Emit(OpCodes.Ret);
                return proxyMethod;
            }).ToList();

            // Create final proxy type.
            var proxyType = proxyBuilder.CreateType();
            ContractToProxyDictionary.Add(typeContractCombination, proxyType);

            // Save dynamic assembly - enable ONLY when testing, to examine results in an IL viewer. Unit tests will fail with this.
            if (saveAssemblyForDebuggingPurposes)
                DynamicAssembly.Save(DynamicAssemblyName);

            // Return proxy of new type.
            return actualObject.GenerateProxy<TContract>(typeContractCombination);
        }

        private static TContract GenerateProxy<TContract>(this object mimicObject, Tuple<Type, Type> combination)
        {
            var proxyType = ContractToProxyDictionary[combination];
            var proxy = DynamicAssembly.CreateInstance(proxyType.FullName);
            var runtimeProxyMimicField = proxyType.GetField(ActualObjectFieldName);
            runtimeProxyMimicField.SetValue(proxy, mimicObject);
            return (TContract)proxy;
        }
    }
}
