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
    [TestFixture(Description="Type Extensions Tests")]
    public class TypeExtensionsTests
    {
        [Test]
        public void TypesWithAttribute()
        {
            var classes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .WithAttribute<TestFixtureAttribute>()
                .ToList();

            Assert.AreNotEqual(0, classes.Count);
        }

        [Test]
        public void TypesWithAttributePredicate()
        {
            var attributes =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .WithAttribute<TestFixtureAttribute>(x => x.Description == "Type Extensions Tests")
                    .ToList();

            Assert.AreEqual(1, attributes.Count);
        }

        [Test]
        public void TypesSelectAttribute()
        {
            var attributes =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .WithAttribute<TestFixtureAttribute>()
                    .First()
                    .SelectAttribute<TestFixtureAttribute>()
                    .ToList();

            Assert.AreEqual(1, attributes.Count);
        }

        [Test]
        public void MethodsWithAttribute()
        {
            var methods = Assembly.GetExecutingAssembly()
                .GetTypes()
                .GetMethods()
                .WithAttribute<TestAttribute>()
                .ToList();

            Assert.IsTrue(methods.Count >= 4);
        }

        [Test(Description = "This specific test")]
        public void MethodsWithAttributePredicate()
        {
            var methods = Assembly.GetExecutingAssembly()
                .GetTypes()
                .GetMethods()
                .WithAttribute<TestAttribute>(x => x.Description == "This specific test")
                .ToList();

            Assert.AreEqual(1, methods.Count);
        }
    }
}
