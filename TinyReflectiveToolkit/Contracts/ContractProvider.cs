/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
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
    /// A class that provides supports for runtime-checked type contracts.
    /// </summary>
    public sealed class ContractProvider
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
        private readonly Dictionary<Tuple<Type, Type>, ProxyInfo> _knownSatisfiedContracts =
            new Dictionary<Tuple<Type, Type>, ProxyInfo>();
        private readonly HashSet<Tuple<Type, Type>> _knownFailingContracts = 
            new HashSet<Tuple<Type, Type>>();

        /// <summary>
        /// Creates a ContractProvider using the default configuration.
        /// </summary>
        public ContractProvider()
            : this(Guid.NewGuid().ToString().Replace("-", ""))
        {
        }
        internal ContractProvider(string identifier)
        {
            _dynamicAssembly =
                Thread.GetDomain()
                    .DefineDynamicAssembly(new AssemblyName("TinyReflectiveToolkit-Dynamic-" + identifier),
                        AssemblyBuilderAccess.RunAndSave);
            _dynamicAssemblyName = "Dynamic-" + identifier + ".dll";
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
            // Check arguments.
            var contract = typeof(TContract);
            if (!contract.IsInterface) throw new NotSupportedException("TContract must be an interface type.");
            if (!contract.IsPublic) throw new NotSupportedException(contract.Name + " must be public.");
            if (!type.IsPublic) throw new NotSupportedException(type.Name + " must be public.");

            var combination = new Tuple<Type, Type>(type, contract);

            // Check if there is an existing proxy type for this combination.
            _lock.AcquireReaderLock(LockTimeout);
            var proxy = GetProxyTypeOrNull(type, contract);
            _lock.ReleaseReaderLock();

            if (proxy != null)
                return new Tuple<bool, Type, ProxyInfo>(true, proxy, null);

            // If this combination has been validated before, return the existing analysis data.
            _lock.AcquireReaderLock(LockTimeout);
            var hasBeenValidatedBefore = _knownSatisfiedContracts.ContainsKey(combination);
            if (hasBeenValidatedBefore)
            {
                var knownProxyInfo = _knownSatisfiedContracts[combination];
                _lock.ReleaseReaderLock();
                return new Tuple<bool, Type, ProxyInfo>(true, null, knownProxyInfo);
            }

            // If this combination has been rejected before, return the existing analysis data.
            var hasBeenRejectedBefore = _knownFailingContracts.Contains(combination);
            if (hasBeenRejectedBefore)
            {
                _lock.ReleaseReaderLock();
                return new Tuple<bool, Type, ProxyInfo>(false, null, null);
            }
            _lock.ReleaseReaderLock();

            // If this is a new combination, analyze it.
            var info = new ProxyInfo
            {
                RequiredMethods = contract.GetMethods().WithoutAttribute<ExposeOperatorAttribute>().ToList(),
                RequiredExplicitConversions = contract.GetMethods().WithAttribute<CastAttribute>().ToList(),
                RequiredImplicitConversions = contract.GetMethods().WithAttribute<ImplicitAttribute>().ToList(),
                RequiredLeftSideAdditionOperators = contract.GetMethods()
                    .WithAttribute<AdditionAttribute>(x => x.OperatorSide == OpSide.ThisLeft).ToList(),
                RequiredRightSideAdditionOperators = contract.GetMethods()
                    .WithAttribute<AdditionAttribute>(x => x.OperatorSide == OpSide.ThisRight).ToList(),
                RequiredLeftSideSubtractionOperators = contract.GetMethods()
                    .WithAttribute<SubtractionAttribute>(x => x.OperatorSide == OpSide.ThisLeft).ToList(),
                RequiredRightSideSubtractionOperators = contract.GetMethods()
                    .WithAttribute<SubtractionAttribute>(x => x.OperatorSide == OpSide.ThisRight).ToList(),
                RequiredLeftSideMultiplyOperators = contract.GetMethods()
                    .WithAttribute<MultiplicationAttribute>(x => x.OperatorSide == OpSide.ThisLeft).ToList(),
                RequiredRightSideMultiplyOperators = contract.GetMethods()
                    .WithAttribute<MultiplicationAttribute>(x => x.OperatorSide == OpSide.ThisRight).ToList(),
                RequiredLeftSideDivisionOperators = contract.GetMethods()
                    .WithAttribute<DivisionAttribute>(x => x.OperatorSide == OpSide.ThisLeft).ToList(),
                RequiredRightSideDivisionOperators = contract.GetMethods()
                    .WithAttribute<DivisionAttribute>(x => x.OperatorSide == OpSide.ThisRight).ToList(),
                RequiredLeftSideModulusOperators = contract.GetMethods()
                    .WithAttribute<ModulusAttribute>(x => x.OperatorSide == OpSide.ThisLeft).ToList(),
                RequiredRightSideModulusOperators = contract.GetMethods()
                    .WithAttribute<ModulusAttribute>(x => x.OperatorSide == OpSide.ThisRight).ToList(),
                RequiredLeftSideEqualityOperators = contract.GetMethods()
                    .WithAttribute<EqualityAttribute>(x => x.OperatorSide == OpSide.ThisLeft).ToList(),
                RequiredRightSideEqualityOperators = contract.GetMethods()
                    .WithAttribute<EqualityAttribute>(x => x.OperatorSide == OpSide.ThisRight).ToList(),
                RequiredLeftSideInequalityOperators = contract.GetMethods()
                    .WithAttribute<InequalityAttribute>(x => x.OperatorSide == OpSide.ThisLeft).ToList(),
                RequiredRightSideInequalityOperators = contract.GetMethods()
                    .WithAttribute<InequalityAttribute>(x => x.OperatorSide == OpSide.ThisRight).ToList(),
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
                if (!conversions.Any())
                {
                    var specialOp = SpecialOperations.GetSpecialConversion(type, x.ReturnType);
                    if (specialOp != null) 
                        conversions.Add(specialOp);
                }
                if (!conversions.Any() && type == x.ReturnType)
                    conversions.Add(SpecialOperations.IdentityMarkerMethodInfo);                                        
                return new Tuple<string, MethodInfo, int>(x.Name, conversions.FirstOrDefault(), 0);
            }).ToList();
            info.FoundImplicitConversions = info.RequiredImplicitConversions.Select(x =>
            {
                var conversions = mimicObjectOperators
                    .Where(m => m.ReturnType == x.ReturnType)
                    .Where(m => m.Name == "op_Implicit")
                    .ToList();
                if (!conversions.Any())
                {
                    var specialOp = SpecialOperations.GetSpecialConversion(type, x.ReturnType);
                    if (specialOp != null)
                        conversions.Add(specialOp);
                }
                if (!conversions.Any() && type == x.ReturnType)
                    conversions.Add(SpecialOperations.IdentityMarkerMethodInfo);    
                return new Tuple<string, MethodInfo, int>(x.Name, conversions.FirstOrDefault(), 0);
            }).ToList();
            Action<List<MethodInfo>, List<Tuple<String, MethodInfo, int>>, string, int> act =
                (required, found, op, index) => found.AddRange(required.Select(x =>
                {
                    var otherParameter = x.GetParameters().Single().ParameterType;
                    var operatorMethods = mimicObjectOperators
                        .Where(m => m.ReturnType == x.ReturnType)
                        .Where(m => m.Name == op)
                        .Where(m => m.GetParameters().ElementAt(1 - index).ParameterType == otherParameter)
                        .ToList();
                    return new Tuple<string, MethodInfo, int>(x.Name, operatorMethods.FirstOrDefault(), index);
                }).ToList());

            act(info.RequiredLeftSideAdditionOperators, info.FoundLeftSideAdditionOperators, "op_Addition", 0);
            act(info.RequiredRightSideAdditionOperators, info.FoundRightSideAdditionOperators, "op_Addition", 1);
            act(info.RequiredLeftSideSubtractionOperators, info.FoundLeftSideSubtractionOperators, "op_Subtraction", 0);
            act(info.RequiredRightSideSubtractionOperators, info.FoundRightSideSubtractionOperators, "op_Subtraction", 1);
            act(info.RequiredLeftSideMultiplyOperators, info.FoundLeftSideMultiplyOperators, "op_Multiply", 0);
            act(info.RequiredRightSideMultiplyOperators, info.FoundRightSideMultiplyOperators, "op_Multiply", 1);
            act(info.RequiredLeftSideDivisionOperators, info.FoundLeftSideDivisionOperators, "op_Division", 0);
            act(info.RequiredRightSideDivisionOperators, info.FoundRightSideDivisionOperators, "op_Division", 1);
            act(info.RequiredLeftSideModulusOperators, info.FoundLeftSideModulusOperators, "op_Modulus", 0);
            act(info.RequiredRightSideModulusOperators, info.FoundRightSideModulusOperators, "op_Modulus", 1);
            act(info.RequiredLeftSideEqualityOperators, info.FoundLeftSideEqualityOperators, "op_Equality", 0);
            act(info.RequiredRightSideEqualityOperators, info.FoundRightSideEqualityOperators, "op_Equality", 1);
            act(info.RequiredLeftSideInequalityOperators, info.FoundLeftSideInequalityOperators, "op_Inequality", 0);
            act(info.RequiredRightSideInequalityOperators, info.FoundRightSideInequalityOperators, "op_Inequality", 1);
           
            // If this looks like a wrong contract, cache analysis results, then return them.
            if (!info.IsValid)
            {
                _lock.AcquireWriterLock(LockTimeout);
                _knownFailingContracts.Add(combination);
                _lock.ReleaseWriterLock();
                return new Tuple<bool, Type, ProxyInfo>(false, null, null);                
            }

            // If this looks like a matching contract, cache analysis results, then return them.
            _lock.AcquireWriterLock(LockTimeout);
            _knownSatisfiedContracts.Add(combination, info);
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
                throw new NotSupportedException();
            if (cachedProxy != null)
                return GenerateProxy<TContract>(actualObject, cachedProxy);

            // Start building a new proxy type.
            var guid = Guid.NewGuid().ToString();
            var sanitizedGuid = guid.Replace("-", "");
            var proxyName = ProxyNamespace + "." + contractType.Name + "_Proxy_" + sanitizedGuid;
            var proxyBuilder = _moduleBuilder.DefineType(proxyName, TypeAttributes.Public, null, new[] { contractType });
            var fieldWithActualObject = proxyBuilder.DefineField(ActualObjectFieldName, actualObjectType, FieldAttributes.Public);

            // Implement proxy stubs.
            var proxyStubsForMethods = proxyInfo.FoundMethods.Select(x =>
            {
                var name = x.Name;
                var parameters = x.GetParameters().Select(p => p.ParameterType).ToArray();
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
            var proxyStubsForOperators = proxyInfo.AllFoundOperators
                .Select(x =>
                {
                    if (x.Item2 == SpecialOperations.IdentityMarkerMethodInfo)
                    {
                        var px = proxyBuilder.DefineMethod(x.Item1, ProxyMethodAttributes, actualObjectType, new Type[0]);
                        var gen = px.GetILGenerator();
                        gen.Emit(OpCodes.Ldarg_0);
                        gen.Emit(OpCodes.Ldfld, fieldWithActualObject);
                        gen.Emit(OpCodes.Ret);
                        return px;
                    }

                    var name = x.Item1;
                    var retType = x.Item2.ReturnType;
                    var index = x.Item3;
                    var parameters = x.Item2.GetParameters().Select(p => p.ParameterType).Where((p, i) => i != index).ToArray();
                    var proxyMethod = proxyBuilder.DefineMethod(name, ProxyMethodAttributes, retType, parameters);
                    var generator = proxyMethod.GetILGenerator();
                    for (var i = 0; i < parameters.Count() + 1; i++)
                        if (i < index) generator.Emit(OpCodes.Ldarg, i + 1);
                        else if (i > index) generator.Emit(OpCodes.Ldarg, i);
                        else
                        {
                            generator.Emit(OpCodes.Ldarg_0);
                            generator.Emit(OpCodes.Ldfld, fieldWithActualObject);
                        }
                    generator.EmitCall(OpCodes.Call, x.Item2, null);
                    generator.Emit(OpCodes.Ret);
                    return proxyMethod;
                }).ToList();

            // Create final proxy type.
            _lock.AcquireWriterLock(LockTimeout);
            var proxyType = proxyBuilder.CreateType();
            _contractToProxyDictionary.Add(new Tuple<Type, Type>(actualObjectType, contractType), proxyType);

            // Save dynamic assembly - enable ONLY when debugging, to examine results in an IL viewer. Unit tests will fail with this.
            if (saveAssemblyForDebuggingPurposes)
                _dynamicAssembly.Save(_dynamicAssemblyName);
            _lock.ReleaseWriterLock();

            // Return proxy of new type.
            return GenerateProxy<TContract>(actualObject, proxyType);
        }

    }
}
