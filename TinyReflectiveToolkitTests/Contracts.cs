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
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TinyReflectiveToolkit;
using TinyReflectiveToolkit.Contracts;

namespace TinyReflectiveToolkitTests
{
    [TestFixture]
    public class Contracts
    {
        [Test]
        public void CachedMethodContracts()
        {
            var thingsWithValue = new IValue[]
            {
                new UnrelatedType1().ToContract<IValue>(),
                new UnrelatedType2().ToContract<IValue>(),
                new UnrelatedType3().ToContract<IValue>(),
                new UnrelatedType1().ToContract<IValue>(),
                new Unrelated<int>().ToContract<IValue>(),
                new Unrelated<bool>().ToContract<IValue>(),
                new Unrelated<int>().ToContract<IValue>()
            };

            var sum = thingsWithValue.Sum(x => x.Value());

            Assert.AreEqual(34, sum);
        }

        [Test]
        public void FailingContract()
        {
            try
            {
                var value = new UnrelatedType4().ToContract<IValue>();
                Assert.Fail();
            }
            catch (TypeLoadException)
            {
            }
        }

        [Test]
        public void VoidContract()
        {
            var value = new UnrelatedType4().ToContract<IVoid>();
            value.Value();
        }

        [Test]
        public void ParameterizedContract()
        {
            var value = new UnrelatedType5().ToContract<IParam>();
            Assert.AreEqual(6, value.Value(1, 2, "", 1, 2, ""));
        }

        [Test]
        public void Overloads()
        {
            var value = new UnrelatedType5().ToContract<ITwoMethods>();
            value.Value();
            Assert.AreEqual(10, value.Value(1, 2, "", 3, 4, ""));
        }

        [Test]
        public void ExplicitConversionOperator()
        {
            var value = new UnrelatedType5().ToContract<ICastableToInt32>();
            Assert.AreEqual(1, value.ToInt32());
        }

        [Test]
        public void ImplicitConversionOperator()
        {
            var value = new UnrelatedType5().ToContract<IConvertibleToFloat>();
            Assert.AreEqual(2.5f, value.ToFloat());
        }

        [Test]
        public void GetProperties()
        {
            var obj = new UnrelatedType6().ToContract<IGet>();
            Assert.AreEqual(1, obj.OnlyGet);
        }

        [Test]
        public void SetProperties()
        {
            var obj = new UnrelatedType6().ToContract<ISet>();
            obj.OnlySet = 5;
        }

        [Test]
        public void GetSetProperties()
        {
            var obj = new UnrelatedType6().ToContract<IGetSet>();
            obj.GetSet = 10;
            Assert.AreEqual(10, obj.GetSet);
        }

        [Test]
        public void ContractChecks()
        {
            Assert.IsTrue(new UnrelatedType1().Satisfies<IValue>());
            Assert.IsTrue(new UnrelatedType1().Satisfies<IValue>());
            Assert.IsTrue(new UnrelatedType2().Satisfies<IValue>());
            Assert.IsFalse(new UnrelatedType1().Satisfies<IGet>());
            Assert.IsFalse(new UnrelatedType1().Satisfies<IGet>());
            Assert.IsFalse(new UnrelatedType1().Satisfies<ISet>());
            Assert.IsFalse(new UnrelatedType1().Satisfies<IGetSet>());
        }

    }
    public interface IValue
    {
        int Value();
    }

    public class UnrelatedType1
    {
        public int Value()
        {
            return 1;
        }
    }

    public class UnrelatedType2
    {
        public int Value()
        {
            return 2;
        }
    }

    public class UnrelatedType3
    {
        public int Value()
        {
            return 3;
        }
    }

    public class Unrelated<T>
    {
        public int Value()
        {
            return 9;
        }
    }

    public interface IVoid
    {
        void Value();
    }

    public class UnrelatedType4
    {
        public void Value()
        {
        }
    }

    public interface IParam
    {
        int Value(int a, int b, string c, int d, int e, string f);
    }

    public class UnrelatedType5
    {
        public int Value(int a, int b, string c, int d, int e, string f)
        {
            return a + b + d + e + c.Count() + f.Count();
        }

        public void Value()
        {            
        }

        public static explicit operator int(UnrelatedType5 obj)
        {
            return 1;
        }

        public static implicit operator float(UnrelatedType5 obj)
        {
            return 2.5f;
        }
    }

    public interface ITwoMethods
    {
        int Value(int a, int b, string c, int d, int e, string f);
        void Value();        
    }

    public interface ICastableToInt32
    {
        [ExplicitConversion]
        int ToInt32();
    }

    public interface IConvertibleToFloat
    {
        [ImplicitConversion]
        float ToFloat();
    }

    public class UnrelatedType6
    {
        public int OnlyGet { get { return 1; } }
        public int OnlySet { set { } }
        public int GetSet { get; set; }
    }

    public interface IGet
    {
        int OnlyGet { get; }
    }

    public interface ISet
    {
        int OnlySet { set; }
    }

    public interface IGetSet
    {
        int GetSet { get; set; }
    }
}
