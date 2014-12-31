using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TinyReflectiveToolkit
{
    public static partial class TypeExtensions
    {
        internal static bool HasAttribute<TAttribute, TMemberInfo>(this TMemberInfo memberInfo, bool inherited)
            where TMemberInfo : MemberInfo
            where TAttribute : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(TAttribute), inherited).Any();
        }

        /// <summary>
        /// Checks whether the provided member has an attribute of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TMemberInfo"></typeparam>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static bool HasAttribute<TAttribute, TMemberInfo>(this TMemberInfo memberInfo)
            where TMemberInfo : MemberInfo
            where TAttribute : Attribute
        {
            return HasAttribute<TAttribute, TMemberInfo>(memberInfo, false);
        }

        /// <summary>
        /// Checks whether the provided type has an attribute of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return HasAttribute<TAttribute, Type>(type);
        }

        /// <summary>
        /// Checks whether the provided method has an attribute of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static bool HasAttribute<TAttribute>(this MethodInfo methodInfo)
            where TAttribute : Attribute
        {
            return HasAttribute<TAttribute, MethodInfo>(methodInfo);
        }

        /// <summary>
        /// Checks whether the provided field has an attribute of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static bool HasAttribute<TAttribute>(this FieldInfo fieldInfo)
            where TAttribute : Attribute
        {
            return HasAttribute<TAttribute, FieldInfo>(fieldInfo);
        }

        /// <summary>
        /// Checks whether the provided property has an attribute of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool HasAttribute<TAttribute>(this PropertyInfo propertyInfo)
            where TAttribute : Attribute
        {
            return HasAttribute<TAttribute, PropertyInfo>(propertyInfo);
        }
    }
}
