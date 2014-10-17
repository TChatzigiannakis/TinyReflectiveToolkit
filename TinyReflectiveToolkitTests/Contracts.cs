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

namespace TinyReflectiveToolkitTests
{
    [TestFixture]
    public class Contracts
    {
        [Test]
        public void SimpleMethodContract()
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
            value.Value(1, 2, "", 3, 4, "");
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
        void Value(int a, int b, string c, int d, int e, string f);
    }

    public class UnrelatedType5
    {
        public void Value(int a, int b, string c, int d, int e, string f)
        {            
        }
    }
}
