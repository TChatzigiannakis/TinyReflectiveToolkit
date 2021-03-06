﻿/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014-2015  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using System;
using System.Linq;
using TinyReflectiveToolkit;
using TinyReflectiveToolkit.Contracts;
using System.Collections;
using System.Collections.Generic;

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
        public void IsDelegate()
        {
            Assert.IsTrue(typeof (Action<int>).IsDelegate());
            Assert.IsTrue(typeof (Func<string, object>).IsDelegate());
            Assert.IsFalse(typeof (int).IsDelegate());
            Assert.IsFalse(typeof (Expression<Action<int>>).IsDelegate());
        }

        [Test]
        public void IsExtensionMethod()
        {
            Assert.IsFalse(typeof(ContractProvider).GetMethods("CheckIfSatisfies").First().IsExtensionMethod());
            Assert.IsTrue(typeof(TypeExtensions).GetMethods("IsExtensionMethod").First().IsExtensionMethod());
        }

        [Test]
        public void GetExtensionMethods()
        {
            var exts = typeof(Assembly).GetExtensionMethods().ToList();
            Assert.AreNotEqual (0, exts.Count);
            Assert.IsTrue (exts.Any (x => x.Name == "GetLoadableTypes"));
        }
    }
}
