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
        public void WithAttributes()
        {
            var classes = Assembly.GetExecutingAssembly().GetTypes().WithAttribute<TestFixtureAttribute>().ToList();

            Assert.AreNotEqual(0, classes.Count);
        }

        [Test]
        public void SelectAttribute()
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
        public void SelectAttributeWithPredicate()
        {
            var attributes =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .WithAttribute<TestFixtureAttribute>(x => x.Description == "Type Extensions Tests")
                    .ToList();

            Assert.AreEqual(1, attributes.Count);
        }
    }
}
