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
    internal class Contracts
    {
        [Test]
        public void SimpleMethodContract()
        {
            var thingsWithValue = new IValue[]
            {
                new UnrelatedType1().ToContract<IValue>(),
                new UnrelatedType2().ToContract<IValue>(),
                new UnrelatedType3().ToContract<IValue>(),
                new UnrelatedType1().ToContract<IValue>()
            };

            var sum = thingsWithValue.Sum(x => x.Value());

            Assert.AreEqual(7, sum);
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
}
