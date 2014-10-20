using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TinyReflectiveToolkit.Contracts
{
    internal class ProxyInfo
    {
        public Type ProvidedType { get; set; }
        public Type Contract { get; set; }
        public List<MethodInfo> RequiredMethods { get; set; }
        public List<MethodInfo> RequiredExplicitConversions { get; set; }
        public List<MethodInfo> RequiredImplicitConversions { get; set; }
        public List<MethodInfo> FoundMethods { get; set; }
        public List<Tuple<string, MethodInfo>> FoundExplicitConversions { get; set; }
        public List<Tuple<string, MethodInfo>> FoundImplicitConversions { get; set; }
    }
}
