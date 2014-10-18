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
            //contracts.SimpleMethodContract();
            //contracts.FailingContract();
            //contracts.VoidContract();
            //contracts.ParameterizedContract();
            //contracts.Overloads();
            //contracts.ExplicitConversionOperator();
            contracts.ImplicitConversionOperator();
        }

        class ConcreteType
        {
            private IParam obj;

            public int Value(int a, int b, string c, int d, int e, string f)
            {
                return obj.Value(a, b, c, d, e, f);
            }

            public static explicit operator int(ConcreteType c)
            {
                return 1;
            }

            public static implicit operator float(ConcreteType n)
            {
                return 1.0f;
            }

            public int CastMe()
            {
                return (int)this;
            }
        }
    }


}
