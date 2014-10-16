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

        private static readonly ModuleBuilder ModuleBuilder =
            DynamicAssembly.DefineDynamicModule("Contracts", "Dynamic.dll");

        private static readonly Dictionary<Tuple<Type, Type>, Type> Dictionary = 
            new Dictionary<Tuple<Type, Type>, Type>();

        private const string MimicObjectFieldName = "_internalObject";
        private const string ProxyNamespace = "TinyReflectiveToolkit.Contracts";

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

        internal static TContract CreateContractProxyFromObject<TContract>(object mimicObject)
            where TContract : class
        {
            var baseType = typeof(TContract);

            if (!baseType.IsInterface) throw new NotSupportedException("TContract must be an interface type.");
            if (!baseType.IsPublic) throw new NotSupportedException(baseType.Name + " must be public.");
            
            var mimicType = mimicObject.GetType();

            if (!mimicType.IsPublic) throw new NotSupportedException(mimicType.Name + " must be public.");

            var combination = new Tuple<Type, Type>(mimicType, baseType);
            if (Dictionary.ContainsKey(combination))
                return mimicObject.GenerateProxy<TContract>(combination);
            else
            {
                var guid = Guid.NewGuid().ToString();
                var proxyName = ProxyNamespace + "." + baseType.Name + "_" + guid.Replace("-", "");

                var typeBuilder = ModuleBuilder.DefineType(proxyName, TypeAttributes.Public, null, new[] {baseType});

                var mimicObjectField = typeBuilder.DefineField(MimicObjectFieldName, mimicType, FieldAttributes.Public);

                var methodsToImplement = baseType.GetMethods().ToList();
                var actualMethodsForThisObject = methodsToImplement.Select(x =>
                {
                    var name = x.Name;
                    var parameters = x.GetParameters().Select(n => n.ParameterType).ToArray();
                    return mimicType.GetMethod(name, parameters);
                }).ToList();

                var proxyImplementations = actualMethodsForThisObject.Select(x =>
                {
                    var name = x.Name;
                    var parameters = x.GetParameters().Select(n => n.ParameterType).ToArray();
                    var retType = x.ReturnType;
                    var proxyMethod = typeBuilder.DefineMethod(name,
                        MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot |
                        MethodAttributes.Final, retType, parameters);
                    var generator = proxyMethod.GetILGenerator();
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldfld, mimicObjectField);
                    generator.EmitCall(OpCodes.Callvirt, x, null);
                    generator.Emit(OpCodes.Ret);
                    return proxyMethod;
                }).ToList();

                var proxyType = typeBuilder.CreateType();
                Dictionary.Add(combination, proxyType);

                return mimicObject.GenerateProxy<TContract>(combination);
            }
        }

        private static TContract GenerateProxy<TContract>(this object mimicObject, Tuple<Type, Type> combination)
        {
            var proxyType = Dictionary[combination];
            var proxy = DynamicAssembly.CreateInstance(proxyType.FullName);
            var runtimeProxyMimicField = proxyType.GetField(MimicObjectFieldName);
            runtimeProxyMimicField.SetValue(proxy, mimicObject);
            return (TContract)proxy;
        }
    }
}
