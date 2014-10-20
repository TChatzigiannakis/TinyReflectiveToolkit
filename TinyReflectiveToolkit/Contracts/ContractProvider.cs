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

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
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

            var proxy = GetProxyTypeOrNull(type, contract);
            if (proxy != null)
                return new Tuple<bool, Type, ProxyInfo>(true, proxy, null);                

            if (!alwaysGiveProxyInfo && _knownSatisfiedContracts.Contains(combination))
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

            _knownSatisfiedContracts.Add(combination);
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
            _contractToProxyDictionary.Add(new Tuple<Type, Type>(actualObjectType, contractType), proxyType);

            // Save dynamic assembly - enable ONLY when testing, to examine results in an IL viewer. Unit tests will fail with this.
            if (saveAssemblyForDebuggingPurposes)
                _dynamicAssembly.Save(_dynamicAssemblyName);

            // Return proxy of new type.
            return GenerateProxy<TContract>(actualObject, proxyType);
        }
    }
}
