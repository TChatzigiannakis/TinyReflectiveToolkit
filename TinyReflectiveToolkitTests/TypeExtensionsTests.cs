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
    }
}
