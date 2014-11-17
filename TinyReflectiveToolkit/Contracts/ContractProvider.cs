/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using TinyReflectiveToolkit.Contracts.SpecialOps;
using EnumerableExtensions;

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// A class that provides supports for runtime-checked structural type contracts.
    /// </summary>
    public sealed class ContractProvider
    {
        private const string RealInstanceFieldName = "InternalObject";
        private const string NamespaceForProxyTypes = "TinyReflectiveToolkit.Contracts";
        private const MethodAttributes AttributesForProxyMethods = MethodAttributes.Public | MethodAttributes.Virtual |
                                               MethodAttributes.NewSlot | MethodAttributes.Final;

        private readonly AssemblyBuilder _assembly;
        private readonly string _assemblyName;
        private readonly ModuleBuilder _moduleBuilder;
        private readonly ConcurrentDictionary<Tuple<Type, Type>, Type> _contractToProxy =
            new ConcurrentDictionary<Tuple<Type, Type>, Type>();
        private readonly ConcurrentDictionary<Tuple<Type, Type>, ProxyInfo> _knownSatisfiedContracts =
            new ConcurrentDictionary<Tuple<Type, Type>, ProxyInfo>();
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
            _assembly =
                Thread.GetDomain()
                    .DefineDynamicAssembly(new AssemblyName("TinyReflectiveToolkit-Dynamic-" + identifier),
                        AssemblyBuilderAccess.RunAndSave);
            _assemblyName = "Dynamic-" + identifier + ".dll";
            _moduleBuilder = _assembly.DefineDynamicModule("Contracts", _assemblyName);
            
        }

        private TContract GenerateProxy<TContract>(object realInstance, Type proxyType)
        {
            var proxyInstance = _assembly.CreateInstance(proxyType.FullName);
            var runtimeInstanceField = proxyType.GetField(RealInstanceFieldName);
            runtimeInstanceField.SetValue(proxyInstance, realInstance);
            return (TContract)proxyInstance;
        }

        private Type GetProxyTypeOrNull(Type realType, Type contract)
        {
            var combination = new Tuple<Type, Type>(realType, contract);
            Type proxyType = null;
            if (_contractToProxy.ContainsKey(combination))
                proxyType = _contractToProxy[combination];
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
            return CheckIfSatisfies<TContract>(obj.GetType()).Item1;
        } 
        private Tuple<bool, Type, ProxyInfo> CheckIfSatisfies<TContract>(Type realType)
            where TContract : class
        {
            // Check arguments.
            var contract = typeof(TContract);
            if (!contract.IsInterface) throw new NotSupportedException("TContract must be an interface type.");
            if (!contract.IsPublic) throw new NotSupportedException(contract.Name + " must be public.");
            if (!realType.IsPublic) throw new NotSupportedException(realType.Name + " must be public.");

            // Check if we have an existing proxy type for this combination.
            var combination = new Tuple<Type, Type>(realType, contract);
            var proxyType = GetProxyTypeOrNull(realType, contract);
            if (proxyType != null)
                return new Tuple<bool, Type, ProxyInfo>(true, proxyType, null);

            // If there is no existing concrete proxy type but this combination has at least been validated before, return the existing analysis data.
            var combinationHasBeenValidatedBefore = _knownSatisfiedContracts.ContainsKey(combination);
            if (combinationHasBeenValidatedBefore)
            {
                var knownProxyInfo = _knownSatisfiedContracts[combination];
                return new Tuple<bool, Type, ProxyInfo>(true, null, knownProxyInfo);
            }

            // If this combination has been rejected before, return the existing analysis data.
            var combinationHasBeenRejectedBefore = _knownFailingContracts.Contains(combination);
            if (combinationHasBeenRejectedBefore)
                return new Tuple<bool, Type, ProxyInfo>(false, null, null);

            // If this is a new combination, analyze it.
            var clrMethodsInContract = contract.GetInheritedInterfaceMembers().OfType<MethodInfo>().ToArray();
            var proxyInfo = new ProxyInfo
            {
                RequiredRegularMethods = clrMethodsInContract.WithoutAttribute<ExposeOperatorAttribute>().ToList(),
                RequiredExplicitConversions = clrMethodsInContract.WithAttribute<CastAttribute>().ToList(),
                RequiredImplicitConversions = clrMethodsInContract.WithAttribute<ImplicitAttribute>().ToList(),
            };
            proxyInfo.ResolveBinaryOperators<AdditionAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<SubtractionAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<MultiplicationAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<DivisionAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<ModulusAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<EqualityAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<InequalityAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<GreaterThanAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<LessThanAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<GreaterThanOrEqualAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<LessThanOrEqualAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<ExclusiveOrAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<BitwiseOrAttribute>(clrMethodsInContract);
            proxyInfo.ResolveBinaryOperators<BitwiseAndAttribute>(clrMethodsInContract);

            // Match the regular instanced methods.
            proxyInfo.FoundRegularMethods = proxyInfo.RequiredRegularMethods.Select(requiredMethod =>
            {
                var matchingMethod = realType.GetGenericMethod(requiredMethod.Name, requiredMethod.GetParameters(), true);
                if (matchingMethod == null || !requiredMethod.ReturnType.IsAssignableFrom(matchingMethod.ReturnType))
                {
                    proxyInfo.AddIssue("Could not locate a matching implementation for method " + requiredMethod + " in type " + realType + ".");
                    return null;                    
                }
                return matchingMethod;
            }).Except(x => x == null).ToList();

            // Match the conversion operators. 
            var realObjectOperators = realType.GetOperators().ToList();
            proxyInfo.FoundExplicitConversions = proxyInfo.RequiredExplicitConversions.Select(requiredOperator =>
            {
                var matchingOperators = realType.GetExplicitOperators(requiredOperator.ReturnType).ToList();
                if (matchingOperators.IsEmpty())
                {
                    var specialOp = SpecialOperations.GetSpecialConversion(realType, requiredOperator.ReturnType);
                    if (specialOp != null) 
                        matchingOperators.Add(specialOp);
                }
                if (matchingOperators.IsEmpty() && realType == requiredOperator.ReturnType)
                    matchingOperators.Add(SpecialOperations.IdentityMarkerMethodInfo);
                var matchingOperator = matchingOperators.FirstOrDefault();
                if (matchingOperator == null)
                    proxyInfo.AddIssue("Could not locate a matching explicit operator for method " + requiredOperator +
                                       " in type " + realType + ".");
                return new Tuple<string, MethodInfo, int>(requiredOperator.Name, matchingOperator, 0);
            }).ToList();
            proxyInfo.FoundImplicitConversions = proxyInfo.RequiredImplicitConversions.Select(requiredOperator =>
            {
                var matchingOperators = realType.GetImplicitOperators(requiredOperator.ReturnType).ToList();
                if (matchingOperators.IsEmpty())
                {
                    var specialOp = SpecialOperations.GetSpecialConversion(realType, requiredOperator.ReturnType);
                    if (specialOp != null)
                        matchingOperators.Add(specialOp);
                }
                if (matchingOperators.IsEmpty() && realType == requiredOperator.ReturnType)
                    matchingOperators.Add(SpecialOperations.IdentityMarkerMethodInfo);
                var matchingOperator = matchingOperators.FirstOrDefault();
                if (matchingOperator == null)
                    proxyInfo.AddIssue("Could not locate a matching implicit operator for method " + requiredOperator +
                                       " in type " + realType + ".");
                return new Tuple<string, MethodInfo, int>(requiredOperator.Name, matchingOperator, 0);
            }).ToList();

            // Match the binary operators.
            Action<List<MethodInfo>, List<Tuple<String, MethodInfo, int>>, string, int, Type> act =
                (required, found, op, index, opMarker) => found.AddRange(required.Select(x =>
                {
                    var otherParameter = x.GetParameters().Single().ParameterType;
                    var operatorMethods = realObjectOperators
                        .Where(m => x.ReturnType.IsAssignableFrom(m.ReturnType))
                        .Where(m => m.Name == op)
                        .Where(m => m.GetParameters().ElementAt(1 - index).ParameterType.IsAssignableFrom(otherParameter))
                        .ToList();
                    if (operatorMethods.IsEmpty())
                    {
                        var specialOp = SpecialOperations.GetSpecialOperator(opMarker, realType, otherParameter, x.ReturnType, index == 1);
                        if (specialOp != null)
                            operatorMethods.Add(specialOp);
                    }
                    var matchingOperator = operatorMethods.FirstOrDefault();
                    if (matchingOperator == null)
                        proxyInfo.AddIssue("Could not locate a matching " + opMarker.Name.Replace("Attribute", "") +
                                           " operator for method " + required + " in type " + realType + ".");
                    return new Tuple<string, MethodInfo, int>(x.Name, matchingOperator, index);
                }).ToList());
            AddOperatorsWith<AdditionAttribute>(act, proxyInfo);
            AddOperatorsWith<SubtractionAttribute>(act, proxyInfo);
            AddOperatorsWith<MultiplicationAttribute>(act, proxyInfo);
            AddOperatorsWith<DivisionAttribute>(act, proxyInfo);
            AddOperatorsWith<ModulusAttribute>(act, proxyInfo);
            AddOperatorsWith<EqualityAttribute>(act, proxyInfo);
            AddOperatorsWith<InequalityAttribute>(act, proxyInfo);
            AddOperatorsWith<GreaterThanAttribute>(act, proxyInfo);
            AddOperatorsWith<LessThanAttribute>(act, proxyInfo);
            AddOperatorsWith<GreaterThanOrEqualAttribute>(act, proxyInfo);
            AddOperatorsWith<LessThanOrEqualAttribute>(act, proxyInfo);
            AddOperatorsWith<ExclusiveOrAttribute>(act, proxyInfo);
            AddOperatorsWith<BitwiseOrAttribute>(act, proxyInfo);
            AddOperatorsWith<BitwiseAndAttribute>(act, proxyInfo);
           
            // If this looks like a wrong contract, cache analysis results (for faster reference in the future), then return them.
            if (!proxyInfo.IsValid)
            {
                _knownFailingContracts.Add(combination);
                return new Tuple<bool, Type, ProxyInfo>(false, null, proxyInfo);                
            }

            // If this looks like a matching contract, cache analysis results (for faster reference in the future), then return them.
            _knownSatisfiedContracts.AddOrUpdate(combination, proxyInfo, (a, b) => b);
            return new Tuple<bool, Type, ProxyInfo>(true, null, proxyInfo);
        }

        private void AddOperatorsWith<TAttribute>(Action<List<MethodInfo>, List<Tuple<String, MethodInfo, int>>, string, int, Type> act, ProxyInfo info)
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            var name = "op_" + typeof (TAttribute).Name.Replace("Attribute", "");
            if (typeof (TAttribute) == typeof (MultiplicationAttribute)) name = "op_Multiply";
            act(info.GetReqLeft<TAttribute>(), info.GetFoundLeft<TAttribute>(), name, 0, typeof(TAttribute));
            act(info.GetReqRight<TAttribute>(), info.GetFoundRight<TAttribute>(), name, 1, typeof(TAttribute));
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

        private TContract CreateContractProxyFromObject<TContract>(object realInstance, bool saveAssemblyForDebuggingPurposes = false)
            where TContract : class
        {
            var contractType = typeof(TContract);
            var realType = realInstance.GetType();

            // Check if contract is satisfied and if a proxy type already exists.
            var satisfactionCheckResult = CheckIfSatisfies<TContract>(realType);
            var contractIsSatisfied = satisfactionCheckResult.Item1;
            var cachedProxyType = satisfactionCheckResult.Item2;
            var proxyInfo = satisfactionCheckResult.Item3;
            if (!contractIsSatisfied)
                throw new ContractUnsatisfiedException(string.Join(" ", proxyInfo.Issues));
            if (cachedProxyType != null)
                return GenerateProxy<TContract>(realInstance, cachedProxyType);

            // Start building a new proxy type.
            var guid = Guid.NewGuid().ToString();
            var cleanGuid = guid.Replace("-", "");
            var proxyTypeName = NamespaceForProxyTypes + "." + contractType.Name + "_Proxy_" + cleanGuid;
            var proxyTypeBuilder = _moduleBuilder.DefineType(proxyTypeName, TypeAttributes.NotPublic, null, new[] { contractType });
            var realInstanceField = proxyTypeBuilder.DefineField(RealInstanceFieldName, realType, FieldAttributes.Public);

            // Implement proxy method stubs.
            var proxyStubsForMethods = proxyInfo.FoundRegularMethods
                .Select(foundMethod =>
            {
                var requiredMethod = proxyInfo.FoundRegularMethods.Corresponding(foundMethod, proxyInfo.RequiredRegularMethods);
                var proxyMethodParameterTypes = requiredMethod.GetParameters().Select(p => p.ParameterType).ToArray();

                var proxyMethod = proxyTypeBuilder.DefineMethod(foundMethod.Name, AttributesForProxyMethods);
                proxyMethod.SetReturnType(requiredMethod.ReturnType);

                if (foundMethod.IsGenericMethod)
                {
                    var parameterizedParameters = foundMethod.GetParameters().Where(p => p.ParameterType.IsGenericParameter).ToArray();
                    var typeParams = proxyMethod.DefineGenericParameters(parameterizedParameters.Select(p => p.Name).ToArray());
                    var attributes = parameterizedParameters.Select(p => p.ParameterType.GenericParameterAttributes).ToArray();
                    var constraints = parameterizedParameters.Select(p => p.ParameterType.GetGenericParameterConstraints()).ToArray();
                    typeParams.ForEach((tp, index) =>
                    {
                        tp.SetGenericParameterAttributes(attributes[index]);
                        tp.SetBaseTypeConstraint(constraints[index].SingleOrDefault(d => d.IsClass));
                        tp.SetInterfaceConstraints(constraints[index].Where(d => d.IsInterface).ToArray());
                    });
                }
                proxyMethod.SetParameters(proxyMethodParameterTypes);

                var generator = proxyMethod.GetILGenerator();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(realType.IsValueType ? OpCodes.Ldflda : OpCodes.Ldfld, realInstanceField);
                for (var i = 0; i < proxyMethodParameterTypes.Count(); i++)
                {
                    generator.Emit(OpCodes.Ldarg, i + 1);
                    if (proxyMethodParameterTypes[i].IsValueType && !foundMethod.GetParameters()[i].ParameterType.IsValueType)
                        generator.Emit(OpCodes.Box, proxyMethodParameterTypes[i]);
                }
                generator.EmitCall(OpCodes.Callvirt, foundMethod, null);
                if (foundMethod.ReturnType.IsValueType && !requiredMethod.ReturnType.IsValueType)
                    generator.Emit(OpCodes.Box, foundMethod.ReturnType);
                generator.Emit(OpCodes.Ret);
                return proxyMethod;
            }).ToList();

            // Implement operator stubs.
            var proxyStubsForOperators = proxyInfo.AllFoundOperators
                .Select(foundOperator =>
                {
                    if (foundOperator.Item2 == SpecialOperations.IdentityMarkerMethodInfo)
                    {
                        var px = proxyTypeBuilder.DefineMethod(foundOperator.Item1, AttributesForProxyMethods, realType, new Type[0]);
                        var gen = px.GetILGenerator();
                        gen.Emit(OpCodes.Ldarg_0);
                        gen.Emit(OpCodes.Ldfld, realInstanceField);
                        gen.Emit(OpCodes.Ret);
                        return px;
                    }

                    var name = foundOperator.Item1;
                    var index = foundOperator.Item3;
                    var requiredOperator = proxyInfo.AllFoundOperators.Corresponding(foundOperator, proxyInfo.AllRequiredOperators);
                    var proxyMethodParameters = requiredOperator.GetParameters().Select(p => p.ParameterType).ToArray();
                    var proxyMethod = proxyTypeBuilder.DefineMethod(name, AttributesForProxyMethods, requiredOperator.ReturnType, proxyMethodParameters);
                    var foundMethodParameters = foundOperator.Item2.GetParameters().Select(p => p.ParameterType).Where((p, i) => i != index).ToArray();
                    
                    var generator = proxyMethod.GetILGenerator();
                    for (var i = 0; i < proxyMethodParameters.Count() + 1; i++)
                    {
                        if (i < index)
                        {
                            generator.Emit(OpCodes.Ldarg, i + 1);
                            if (proxyMethodParameters[i].IsValueType && !foundMethodParameters[i].IsValueType)
                                generator.Emit(OpCodes.Box, proxyMethodParameters[i]);
                        }
                        else if (i > index)
                        {
                            generator.Emit(OpCodes.Ldarg, i);
                            if (proxyMethodParameters[i - 1].IsValueType && !foundMethodParameters[i - 1].IsValueType)
                                generator.Emit(OpCodes.Box, proxyMethodParameters[i - 1]);
                        }
                        else
                        {
                            generator.Emit(OpCodes.Ldarg_0);
                            generator.Emit(OpCodes.Ldfld, realInstanceField);
                        }
                    }
                    generator.EmitCall(OpCodes.Call, foundOperator.Item2, null);
                    if (foundOperator.Item2.ReturnType.IsValueType && !requiredOperator.ReturnType.IsValueType)
                        generator.Emit(OpCodes.Box, foundOperator.Item2.ReturnType);
                    generator.Emit(OpCodes.Ret);
                    return proxyMethod;
                }).ToList();

            // Create final proxy type.
            var proxyType = proxyTypeBuilder.CreateType();
            var combination = new Tuple<Type, Type>(realType, contractType);
            _contractToProxy.AddOrUpdate(combination, proxyType, (a, b) => b);

            // Save dynamic assembly - enable ONLY when debugging, to examine results in an IL viewer. Unit tests will fail with this.
            if (saveAssemblyForDebuggingPurposes)
                _assembly.Save(_assemblyName);

            // Return proxy of new type.
            return GenerateProxy<TContract>(realInstance, proxyType);
        }

    }
}
