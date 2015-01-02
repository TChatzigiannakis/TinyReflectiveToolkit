/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014-2015  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Linq;
using System.Reflection;

namespace TinyReflectiveToolkit
{
    public static partial class TypeExtensions
    {
        internal static MemberInfo[] GetInheritedInterfaceMembersImpl(this Type type, BindingFlags? bindingAttr = null)
        {
            if (type == null) throw new ArgumentNullException("type");

            var currentMembers = bindingAttr.HasValue ? type.GetMembers(bindingAttr.Value) : type.GetMembers();
            var ghostMembers = type.GetInterfaces()
                .Select(i => bindingAttr.HasValue ? i.GetInheritedInterfaceMembers(bindingAttr.Value) : i.GetInheritedInterfaceMembers())
                .SelectMany(m => m).ToArray();
            return currentMembers.Union(ghostMembers).ToArray();
        }

        /// <summary>
        /// Gets all declared members in an interface, including those declared in its parent interfaces recursively, using the specified binding constraints.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        public static MemberInfo[] GetInheritedInterfaceMembers(this Type type, BindingFlags bindingAttr)
        {
            return type.GetInheritedInterfaceMembersImpl(bindingAttr);
        }

        /// <summary>
        /// Gets all declared members in an interface, including those declared in its parent interfaces recursively.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MemberInfo[] GetInheritedInterfaceMembers(this Type type)
        {
            return type.GetInheritedInterfaceMembersImpl();
        }

        /// <summary>
        /// Gets all declared methods in an interface, including those declared in its parent interfaces recursively, using the specified binding constraints.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        public static MethodInfo[] GetInheritedInterfaceMethods(this Type type, BindingFlags bindingAttr)
        {
            return type.GetInheritedInterfaceMembers(bindingAttr).OfType<MethodInfo>().ToArray();
        }

        /// <summary>
        /// Gets all declared methods in an interface, including those declared in its parent interfaces recursively.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MethodInfo[] GetInheritedInterfaceMethods(this Type type)
        {
            return type.GetInheritedInterfaceMembers().OfType<MethodInfo>().ToArray();
        }

        /// <summary>
        /// Gets all declared properties in an interface, including those declared in its parent interfaces recursively, using the specified binding constraints.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetInheritedInterfaceProperties(this Type type, BindingFlags bindingAttr)
        {
            return type.GetInheritedInterfaceMembers(bindingAttr).OfType<PropertyInfo>().ToArray();
        }

        /// <summary>
        /// Gets all declared properties in an interface, including those declared in its parent interfaces recursively, using the specified binding constraints.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetInheritedInterfaceProperties(this Type type)
        {
            return type.GetInheritedInterfaceMembers().OfType<PropertyInfo>().ToArray();
        }
    }
}
