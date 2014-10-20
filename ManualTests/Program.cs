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
            contracts.CachedMethodContracts();
            //contracts.FailingContract();
            //contracts.VoidContract();
            //contracts.ParameterizedContract();
            //contracts.Overloads();
            //contracts.ExplicitConversionOperator();
            //contracts.ImplicitConversionOperator();
            //contracts.GetProperties();
        }        
    }
}
