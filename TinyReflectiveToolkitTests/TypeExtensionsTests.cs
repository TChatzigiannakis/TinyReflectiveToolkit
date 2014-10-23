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

using System.Reflection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyReflectiveToolkit;

namespace TinyReflectiveToolkitTests
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    internal sealed class MyAttribute : Attribute
    {
        public string Description { get; private set; }

        public MyAttribute(string description = "")
        {
            Description = description;
        }
    }

    [My("Type Extensions Tests")]
    [TestFixture]
    public class TypeExtensionsTests
    {
        [My("1")]
        public int One { get { return 1; } }
        [My("2")]
        public int Two { get { return 2; } }

        [Test]
        public void TypesWithAttribute()
        {
            var classes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .WithAttribute<MyAttribute>()
                .ToList();

            Assert.AreNotEqual(0, classes.Count);
        }

        [Test]
        public void TypesWithAttributeAndPredicate()
        {
            var attributes =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .WithAttribute<MyAttribute>(x => x.Description == "Type Extensions Tests")
                    .ToList();

            Assert.AreEqual(1, attributes.Count);
        }

        [Test]
        public void TypesSelectAttribute()
        {
            var attributes =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .WithAttribute<MyAttribute>()
                    .First()
                    .SelectAttribute<MyAttribute>()
                    .ToList();

            Assert.AreEqual(1, attributes.Count);
        }

        [My]
        [Test]
        public void MethodsWithAttribute()
        {
            var methods = Assembly.GetExecutingAssembly()
                .GetTypes()
                .GetMethods()
                .WithAttribute<MyAttribute>()
                .ToList();

            Assert.IsTrue(methods.Count >= 3);
        }

        [My]
        [Test]
        public void MethodsWithTwoAttributes()
        {
            var methods = Assembly.GetExecutingAssembly()
                .GetTypes()
                .GetMethods()
                .WithAttribute<TestAttribute>()
                .WithAttribute<MyAttribute>()
                .ToList();

            Assert.IsTrue(methods.Count >= 3);
        }

        [My("This specific test")]
        [Test]
        public void MethodsWithAttributeAndPredicate()
        {
            var methods = Assembly.GetExecutingAssembly()
                .GetTypes()
                .GetMethods()
                .WithAttribute<MyAttribute>(x => x.Description == "This specific test")
                .ToList();

            Assert.AreEqual(1, methods.Count);
        }

        [Test]
        public void PropertiesWithAttribute()
        {
            var properties = Assembly.GetExecutingAssembly()
                .GetTypes()
                .GetProperties()
                .WithAttribute<MyAttribute>()
                .ToList();

            Assert.AreEqual(2, properties.Count);
        }

        [Test]
        public void PropertiesWithAttributeAndPredicate()
        {
            var properties = Assembly.GetExecutingAssembly()
                .GetTypes()
                .GetProperties()
                .WithAttribute<MyAttribute>(x => x.Description == "1")
                .ToList();

            Assert.AreEqual(1, properties.Count);
        }

        [Test]
        public void InvariantCheck()
        {
            Assert.IsTrue(typeof (int).CanBeCastTo(typeof (int), Variance.Invariant));
        }

        [Test]
        public void CovariantCheck()
        {
            Assert.IsTrue(typeof (int).CanBeCastTo(typeof (object), Variance.Covariant));
        }

        [Test]
        public void ContravariantCheck()
        {
            Assert.IsTrue(typeof (object).CanBeCastTo(typeof (int), Variance.Contravariant));
        }
    }
}
