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
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using EnumerableExtensions;

namespace TinyReflectiveToolkit.Contracts
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

        private static readonly HashSet<Tuple<Type, Type>> KnownSatisfiedContracts =
            new HashSet<Tuple<Type, Type>>();
        
        private const MethodAttributes ProxyMethodAttributes = MethodAttributes.Public | MethodAttributes.Virtual |
                                               MethodAttributes.NewSlot | MethodAttributes.Final; 

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

        /// <summary>
        /// Checks whether the runtime type of an object structurally satisfies a specified contract.
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Satisfies<TContract>(this object obj)
            where TContract : class
        {
            return obj.GetType().Satisfies<TContract>(false).Item1;
        }

        private static Tuple<bool, Type, ProxyInfo> Satisfies<TContract>(this Type type, bool alwaysGiveProxyInfo)
            where TContract : class
        {
            var contract = typeof(TContract);
            if (!contract.IsInterface) throw new NotSupportedException("TContract must be an interface type.");
            if (!contract.IsPublic) throw new NotSupportedException(contract.Name + " must be public.");
            if (!type.IsPublic) throw new NotSupportedException(type.Name + " must be public.");

            var combination = new Tuple<Type, Type>(type, contract);

            var proxy = GetProxyTypeOrNull(type, contract);
            if (proxy != null)
                return new Tuple<bool, Type, ProxyInfo>(true, proxy, null);                

            if (!alwaysGiveProxyInfo && KnownSatisfiedContracts.Contains(combination))
                return new Tuple<bool, Type, ProxyInfo>(true, null, null);

            var info = new ProxyInfo
            {
                RequiredMethods = contract.GetMethods()
                    .WithoutAttribute<ExplicitConversionAttribute>()
                    .WithoutAttribute<ImplicitConversionAttribute>()
                    .ToList(),
                RequiredExplicitConversions = contract.GetMethods()
                    .WithAttribute<ExplicitConversionAttribute>()
                    .ToList(),
                RequiredImplicitConversions = contract.GetMethods()
                    .WithAttribute<ImplicitConversionAttribute>()
                    .ToList()
            };

            info.FoundMethods = info.RequiredMethods.Select(x =>
            {
                var name = x.Name;
                var parameters = x.GetParameters().Select(n => n.ParameterType).ToArray();
                return type.GetMethod(name, parameters);
            }).Except(x => x == null).ToList();

            var mimicObjectOperators = type.GetMethods()
                .Where(m => m.IsStatic)
                .Where(m => m.Name.StartsWith("op_"))
                .ToList();
            info.FoundExplicitConversions = info.RequiredExplicitConversions.Select(x =>
            {
                var conversions = mimicObjectOperators
                    .Where(m => m.ReturnType == x.ReturnType)
                    .Where(m => m.Name == "op_Explicit")
                    .ToList();
                return new Tuple<string, MethodInfo>(x.Name, conversions.FirstOrDefault());
            }).Except(x => x == null).ToList();
            info.FoundImplicitConversions = info.RequiredImplicitConversions.Select(x =>
            {
                var conversions = mimicObjectOperators
                    .Where(m => m.ReturnType == x.ReturnType)
                    .Where(m => m.Name == "op_Implicit")
                    .ToList();
                return new Tuple<string, MethodInfo>(x.Name, conversions.FirstOrDefault());
            }).Except(x => x == null).ToList();

            if (info.RequiredMethods.Count != info.FoundMethods.Count ||
                info.RequiredExplicitConversions.Count != info.FoundExplicitConversions.Count ||
                info.RequiredImplicitConversions.Count != info.FoundImplicitConversions.Count)
                return new Tuple<bool, Type, ProxyInfo>(false, null, null);

            KnownSatisfiedContracts.Add(combination);
            return new Tuple<bool, Type, ProxyInfo>(true, null, info);
        }

        private static TContract CreateContractProxyFromObject<TContract>(object actualObject, bool saveAssemblyForDebuggingPurposes = false)
            where TContract : class
        {
            var contractType = typeof (TContract);
            var actualObjectType = actualObject.GetType();

            // Check if contract is satisfied and if a proxy type already exists.
            var satisfactionCheckResult = actualObjectType.Satisfies<TContract>(true);
            var satisfies = satisfactionCheckResult.Item1;
            var cachedProxy = satisfactionCheckResult.Item2;
            var proxyInfo = satisfactionCheckResult.Item3;
            if (!satisfies) 
                throw new InvalidOperationException();
            if (cachedProxy != null)
                return actualObject.GenerateProxy<TContract>(cachedProxy);

            // Start building a new proxy type.
            var guid = Guid.NewGuid().ToString();
            var sanitizedGuid = guid.Replace("-", "");
            var proxyName = ProxyNamespace + "." + contractType.Name + "_" + sanitizedGuid;
            var proxyBuilder = ModuleBuilder.DefineType(proxyName, TypeAttributes.Public, null, new[] { contractType });
            var fieldWithActualObject = proxyBuilder.DefineField(ActualObjectFieldName, actualObjectType, FieldAttributes.Public);
            
            // Implement proxy stubs.
            var proxyStubsForMethods = proxyInfo.FoundMethods.Select(x =>
            {
                var name = x.Name;
                var parameters = x.GetParameters().Select(n => n.ParameterType).ToArray();
                var retType = x.ReturnType;
                var proxyMethod = proxyBuilder.DefineMethod(name, ProxyMethodAttributes, retType, parameters);
                var generator = proxyMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, fieldWithActualObject);
                for (var i = 0; i < parameters.Count(); i++)
                    generator.Emit(OpCodes.Ldarg, i + 1);
                generator.EmitCall(OpCodes.Callvirt, x, null);
                generator.Emit(OpCodes.Ret);
                return proxyMethod;
            }).ToList();
            var proxyStubsForOperators = proxyInfo.FoundExplicitConversions.Concat(proxyInfo.FoundImplicitConversions)
                .Select(x =>
            {
                var name = x.Item1;
                var retType = x.Item2.ReturnType;
                var proxyMethod = proxyBuilder.DefineMethod(name, ProxyMethodAttributes, retType, new Type[0]);
                var generator = proxyMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, fieldWithActualObject);
                generator.EmitCall(OpCodes.Call, x.Item2, null);
                generator.Emit(OpCodes.Ret);
                return proxyMethod;
            }).ToList();

            // Create final proxy type.
            var proxyType = proxyBuilder.CreateType();
            ContractToProxyDictionary.Add(new Tuple<Type, Type>(actualObjectType, contractType), proxyType);

            // Save dynamic assembly - enable ONLY when testing, to examine results in an IL viewer. Unit tests will fail with this.
            if (saveAssemblyForDebuggingPurposes)
                DynamicAssembly.Save(DynamicAssemblyName);

            // Return proxy of new type.
            return actualObject.GenerateProxy<TContract>(proxyType);
        }

        private static TContract GenerateProxy<TContract>(this object mimicObject, Type proxyType)
        {
            var proxy = DynamicAssembly.CreateInstance(proxyType.FullName);
            var runtimeProxyMimicField = proxyType.GetField(ActualObjectFieldName);
            runtimeProxyMimicField.SetValue(proxy, mimicObject);
            return (TContract)proxy;
        }

        private static Type GetProxyTypeOrNull(Type type, Type contract)
        {
            var combination = new Tuple<Type, Type>(type, contract);
            if (ContractToProxyDictionary.ContainsKey(combination))
                return ContractToProxyDictionary[combination];
            return null;
        }

    }
}
