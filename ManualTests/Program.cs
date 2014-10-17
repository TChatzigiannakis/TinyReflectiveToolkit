using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyReflectiveToolkit;
using TinyReflectiveToolkitTests;

namespace ManualTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var contracts = new Contracts();
            contracts.SimpleMethodContract();
            contracts.FailingContract();
            contracts.VoidContract();
            contracts.ParameterizedContract();
        }

        class ConcreteType
        {
            private IParam obj;

            public void Value(int a, int b, string c, int d, int e, string f)
            {
                obj.Value(a, b, c, d, e, f);
            }
        }
    }


}
