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
using System.Text;
using System.Threading;
using EnumerableExtensions;

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// A class that provides supports for runtime-checked type contracts.
    /// </summary>
    public class ContractProvider
    {
        private const string ActualObjectFieldName = "InternalObject";
        private const string ProxyNamespace = "TinyReflectiveToolkit.Contracts";
        private const MethodAttributes ProxyMethodAttributes = MethodAttributes.Public | MethodAttributes.Virtual |
                                               MethodAttributes.NewSlot | MethodAttributes.Final;
        private const int LockTimeout = 1000;

        private readonly ReaderWriterLock _lock = new ReaderWriterLock();
        private readonly AssemblyBuilder _dynamicAssembly;
        private readonly string _dynamicAssemblyName;
        private readonly ModuleBuilder _moduleBuilder;
        private readonly Dictionary<Tuple<Type, Type>, Type> _contractToProxyDictionary =
            new Dictionary<Tuple<Type, Type>, Type>();
        private readonly HashSet<Tuple<Type, Type>> _knownSatisfiedContracts =
            new HashSet<Tuple<Type, Type>>();

        /// <summary>
        /// Creates a ContractProvider using the default configuration.
        /// </summary>
        public ContractProvider()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            _dynamicAssembly =
                Thread.GetDomain()
                    .DefineDynamicAssembly(new AssemblyName("TinyReflectiveToolkit-Dynamic-" + guid),
                        AssemblyBuilderAccess.RunAndSave);
            _dynamicAssemblyName = "Dynamic-" + guid + ".dll";
            _moduleBuilder = _dynamicAssembly.DefineDynamicModule("Contracts", _dynamicAssemblyName);
        }

        private TContract GenerateProxy<TContract>(object mimicObject, Type proxyType)
        {
            var proxy = _dynamicAssembly.CreateInstance(proxyType.FullName);
            var runtimeProxyMimicField = proxyType.GetField(ActualObjectFieldName);
            runtimeProxyMimicField.SetValue(proxy, mimicObject);
            return (TContract)proxy;
        }

        private Type GetProxyTypeOrNull(Type type, Type contract)
        {
            var combination = new Tuple<Type, Type>(type, contract);
            Type proxyType = null;
            if (_contractToProxyDictionary.ContainsKey(combination))
                proxyType = _contractToProxyDictionary[combination];
            return proxyType;
        }

        /// <summary>
        /// Checks whether the provided object satisfies the specified contract.
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CheckIfSatisfies<TContract>(object obj)
            where TContract : class
        {
            return CheckIfSatisfies<TContract>(obj.GetType(), false).Item1;
        } 
        private Tuple<bool, Type, ProxyInfo> CheckIfSatisfies<TContract>(Type type, bool alwaysGiveProxyInfo)
            where TContract : class
        {
            var contract = typeof(TContract);
            if (!contract.IsInterface) throw new NotSupportedException("TContract must be an interface type.");
            if (!contract.IsPublic) throw new NotSupportedException(contract.Name + " must be public.");
            if (!type.IsPublic) throw new NotSupportedException(type.Name + " must be public.");

            var combination = new Tuple<Type, Type>(type, contract);

            _lock.AcquireReaderLock(LockTimeout);
            var proxy = GetProxyTypeOrNull(type, contract);
            _lock.ReleaseReaderLock();

            if (proxy != null)
                return new Tuple<bool, Type, ProxyInfo>(true, proxy, null);                

            if (!alwaysGiveProxyInfo && _knownSatisfiedContracts.Contains(combination))
                return new Tuple<bool, Type, ProxyInfo>(true, null, null);

            _lock.AcquireWriterLock(LockTimeout);

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
            }).ToList();
            info.FoundImplicitConversions = info.RequiredImplicitConversions.Select(x =>
            {
                var conversions = mimicObjectOperators
                    .Where(m => m.ReturnType == x.ReturnType)
                    .Where(m => m.Name == "op_Implicit")
                    .ToList();
                return new Tuple<string, MethodInfo>(x.Name, conversions.FirstOrDefault());
            }).ToList();
            
            if(!info.IsValid)
                return new Tuple<bool, Type, ProxyInfo>(false, null, null);

            _knownSatisfiedContracts.Add(combination);
            _lock.ReleaseWriterLock();

            return new Tuple<bool, Type, ProxyInfo>(true, null, info);
        }

        /// <summary>
        /// Returns an instance of the specified contract that delegates member accesses to the provided object. 
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public TContract ConvertToContractInstance<TContract>(object obj)
            where TContract : class
        {
            return CreateContractProxyFromObject<TContract>(obj);
        }

        private TContract CreateContractProxyFromObject<TContract>(object actualObject, bool saveAssemblyForDebuggingPurposes = false)
            where TContract : class
        {
            var contractType = typeof(TContract);
            var actualObjectType = actualObject.GetType();

            // Check if contract is satisfied and if a proxy type already exists.
            var satisfactionCheckResult = CheckIfSatisfies<TContract>(actualObjectType, true);
            var satisfies = satisfactionCheckResult.Item1;
            var cachedProxy = satisfactionCheckResult.Item2;
            var proxyInfo = satisfactionCheckResult.Item3;
            if (!satisfies)
                throw new InvalidOperationException();
            if (cachedProxy != null)
                return GenerateProxy<TContract>(actualObject, cachedProxy);

            // Start building a new proxy type.
            var guid = Guid.NewGuid().ToString();
            var sanitizedGuid = guid.Replace("-", "");
            var proxyName = ProxyNamespace + "." + contractType.Name + "_" + sanitizedGuid;
            var proxyBuilder = _moduleBuilder.DefineType(proxyName, TypeAttributes.Public, null, new[] { contractType });
            var fieldWithActualObject = proxyBuilder.DefineField(ActualObjectFieldName, actualObjectType, FieldAttributes.Public);

            // Implement proxy stubs.
            var proxyStubsForMethods = proxyInfo.FoundMethods.Select(x =>
            {
                var name = x.Name;
                var parameters = x.GetParameters().Select(n => n.ParameterType).ToArray();
                var retType = x.ReturnType;
                var proxyMethod = proxyBuilder.DefineMethod(name, ProxyMethodAttributes, retType, parameters);
                var generator = proxyMethod.GetILGenerator();
                GenerateStub(generator, fieldWithActualObject, x, true, parameters);
                return proxyMethod;
            }).ToList();
            var proxyStubsForOperators = proxyInfo.FoundExplicitConversions.Concat(proxyInfo.FoundImplicitConversions)
                .Select(x =>
                {
                    var name = x.Item1;
                    var retType = x.Item2.ReturnType;
                    var proxyMethod = proxyBuilder.DefineMethod(name, ProxyMethodAttributes, retType, new Type[0]);
                    var generator = proxyMethod.GetILGenerator();
                    GenerateStub(generator, fieldWithActualObject, x.Item2, false);
                    return proxyMethod;
                }).ToList();

            // Create final proxy type.
            _lock.AcquireWriterLock(LockTimeout);
            var proxyType = proxyBuilder.CreateType();
            _contractToProxyDictionary.Add(new Tuple<Type, Type>(actualObjectType, contractType), proxyType);

            // Save dynamic assembly - enable ONLY when testing, to examine results in an IL viewer. Unit tests will fail with this.
            if (saveAssemblyForDebuggingPurposes)
                _dynamicAssembly.Save(_dynamicAssemblyName);
            _lock.ReleaseWriterLock();

            // Return proxy of new type.
            return GenerateProxy<TContract>(actualObject, proxyType);
        }

        private static void GenerateStub(ILGenerator generator, FieldInfo field, MethodInfo method, bool callVirt, Type[] parameters = null)
        {
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, field);
            if(parameters != null)
                for (var i = 0; i < parameters.Count(); i++)
                    generator.Emit(OpCodes.Ldarg, i + 1);
            generator.EmitCall(callVirt ? OpCodes.Callvirt : OpCodes.Call, method, null);
            generator.Emit(OpCodes.Ret);
        }
    }
}
