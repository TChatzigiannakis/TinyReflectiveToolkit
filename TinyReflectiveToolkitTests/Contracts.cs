﻿/*
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
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TinyReflectiveToolkit;
using TinyReflectiveToolkit.Contracts;
using TinyReflectiveToolkit.Contracts.Premade;

namespace TinyReflectiveToolkitTests
{
    [TestFixture]
    public class Contracts
    {
        [Test]
        public void CachedMethodContracts()
        {
            var thingsWithValue = new IIntMethod[]
            {
                new UnrelatedType1().ToContract<IIntMethod>(),
                new UnrelatedType2().ToContract<IIntMethod>(),
                new UnrelatedType3().ToContract<IIntMethod>(),
                new UnrelatedType1().ToContract<IIntMethod>(),
                new Unrelated<int>().ToContract<IIntMethod>(),
                new Unrelated<bool>().ToContract<IIntMethod>(),
                new Unrelated<int>().ToContract<IIntMethod>()
            };

            var sum = thingsWithValue.Sum(x => x.Value());

            Assert.AreEqual(34, sum);
        }

        [Test]
        public void FailingContract()
        {
            try
            {
                var value = new UnrelatedType4().ToContract<IIntMethod>();
                Assert.Fail();
            }
            catch (TypeLoadException)
            {
            }
        }

        [Test]
        public void VoidContract()
        {
            var value = new UnrelatedType4().ToContract<IVoidMethod>();
            value.Value();
        }

        [Test]
        public void ParameterizedContract()
        {
            var value = new UnrelatedType5().ToContract<IParameterizedMethod>();
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
        public void GenericContract()
        {
            var value = new UnrelatedType5().ToContract<ICastableTo<int>>();
            Assert.AreEqual(1, value.Cast());
        }

        [Test]
        public void GetProperties()
        {
            var obj = new UnrelatedType6().ToContract<IGetter>();
            Assert.AreEqual(1, obj.OnlyGet);
        }

        [Test]
        public void SetProperties()
        {
            var obj = new UnrelatedType6().ToContract<ISetter>();
            obj.OnlySet = 5;
        }

        [Test]
        public void GetSetProperties()
        {
            var obj = new UnrelatedType6().ToContract<IGetterAndSetter>();
            obj.GetSet = 10;
            Assert.AreEqual(10, obj.GetSet);
        }

        [Test]
        public void ContractChecks()
        {
            Assert.IsTrue(new UnrelatedType1().Satisfies<IIntMethod>());
            Assert.IsTrue(new UnrelatedType1().Satisfies<IIntMethod>());
            Assert.IsTrue(new UnrelatedType2().Satisfies<IIntMethod>());
            Assert.IsFalse(new UnrelatedType1().Satisfies<IGetter>());
            Assert.IsFalse(new UnrelatedType1().Satisfies<IGetter>());
            Assert.IsFalse(new UnrelatedType1().Satisfies<ISetter>());
            Assert.IsFalse(new UnrelatedType1().Satisfies<IGetterAndSetter>());
        }

        [Test]
        public void AdditionLeftSide()
        {
            var five = new UnrelatedType7 {Value = 5}.ToContract<IAddableLeft>();
            var sum = five.Add(3);
            Assert.AreEqual(8, sum);
        }
        [Test]
        public void AdditionRightSide()
        {
            var nine = new UnrelatedType7 { Value = 9 }.ToContract<IAddableRight>();
            var sum = nine.Add(4);
            Assert.AreEqual(13, sum);
        }

        [Test]
        public void SubtractionBothSides()
        {
            var ten = new UnrelatedType7 {Value = 10}.ToContract<ISubtractable>();
            Assert.AreEqual(3, ten.Subtract(7));
            Assert.AreEqual(4, ten.SubtractFrom(14));
        }

        [Test]
        public void MultiplicationGeneric()
        {
            var nine = new UnrelatedType7 {Value = 9}.ToContract<IMultipliable<int>>();
            Assert.AreEqual(81, nine.MultiplyBy(9));
        }

        [Test]
        public void DivisionGeneric()
        {
            var twelve = new UnrelatedType7 {Value = 12}.ToContract<IDividable<int, int>>();
            Assert.AreEqual(3, twelve.DivideBy(4));
        }

        [Test]
        public void CastableToInt()
        {
            Assert.AreEqual(12, (12).ToContract<ICastableTo<int>>().Cast());
            Assert.AreEqual(12, (12.0f).ToContract<ICastableTo<int>>().Cast());
            Assert.AreEqual(12, (12.0).ToContract<ICastableTo<int>>().Cast());
            Assert.AreEqual(12, (12m).ToContract<ICastableTo<int>>().Cast());
            Assert.AreEqual(12, ((byte)12).ToContract<ICastableTo<int>>().Cast());
        }

        [Test]
        public void SelfCastable()
        {
            var nn = new UnrelatedType7 {Value = 99};
            var contract = nn.ToContract<ICastableTo<UnrelatedType7>>();
            var self = contract.Cast();
            Assert.AreSame(nn, self);
        }

        [Test]
        public void Modulus()
        {
            var ten = new UnrelatedType7 {Value = 10}.ToContract<IModulusOperator>();
            Assert.AreEqual(1, ten.Modulus(3));
        }

        [Test]
        public void EqualityAndInequality()
        {
            var eight = new UnrelatedType7 {Value = 8}.ToContract<IComparableTo1<int>>();
            Assert.IsTrue(eight.Equals(8));
            Assert.IsFalse(eight.Equals(9));
            Assert.IsTrue(eight.NotEqualTo(9));
            Assert.IsFalse(eight.Equals(9));
        }

        [Test]
        public void GenericMethods()
        {
            var obj = new UnrelatedType8();
            Assert.IsTrue(obj.Satisfies<IGenericMethod1>());
            var cObj = obj.ToContract<IGenericMethod1>();
            var cObjStr = cObj.GetGeneric(obj);
            Assert.IsTrue(obj.ToString() == cObjStr);
        }

        [Test]
        public void MoreGenericMethods()
        {
            var obj = new UnrelatedType8();
            Assert.IsTrue(obj.Satisfies<IGenericMethod2>());
            var cObj = obj.ToContract<IGenericMethod2>();
            var cObjStr = cObj.GetGeneric(obj, 0, obj);
            Assert.IsTrue(obj.ToString() + obj.ToString() == cObjStr);

        }

        [Test]
        public void ContractInheritance()
        {
            var obj = new UnrelatedType8();
            Assert.IsTrue(obj.Satisfies<ICastableTo<int>>());
            Assert.IsTrue(obj.Satisfies<ICastableToInt>());
            Assert.AreEqual((int) obj, obj.ToContract<ICastableToInt>().Cast());
        }

        [Test]
        public void GreaterThan()
        {
            var obj = new UnrelatedType8().ToContract<IComparableTo2<int>>();
            Assert.IsFalse(obj.GreaterThan(1));
        }

        [Test]
        public void LessThan()
        {
            var obj = new UnrelatedType8().ToContract<IComparableTo3<int>>();
            Assert.IsTrue(obj.LessThan(1));
        }
    }

}
