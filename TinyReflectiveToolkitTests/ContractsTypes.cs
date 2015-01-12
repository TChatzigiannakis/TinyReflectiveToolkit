/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014-2015  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Linq;

namespace TinyReflectiveToolkitTests
{
    public enum OneEnum
    {
        One, Two, Three
    }

    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType1
    {
        public static int StaticValue()
        {
            return 1;
        }

        public static int StaticValue(int v)
        {
            return v;
        }

        public int Value()
        {
            return 1;
        }
    }

    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType2
    {
        public static int StaticValue()
        {
            return 2;
        }

        public static int StaticValue(int v)
        {
            return 2 * v;
        }

        public int Value()
        {
            return 2;
        }
    }

    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType3
    {
        public static int StaticValue(int v)
        {
            return v;
        }

        public int Value()
        {
            return 3;
        }
    }

    /// <summary>
    /// Test type.
    /// </summary>
    public class Unrelated<T>
    {
        public int Value()
        {
            return 9;
        }
    }
    
    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType4
    {
        public static float StaticValue(int v)
        {
            return v;
        }

        public void Value()
        {
        }
    }
    
    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType5
    {
        public static string StaticValue(int v)
        {
            return v.ToString();
        }

        public int Value(int a, int b, string c, int d, int e, string f)
        {
            return a + b + d + e + c.Count() + f.Count();
        }

        public void Value()
        {
        }

        public static explicit operator int(UnrelatedType5 obj)
        {
            return 1;
        }

        public static implicit operator float(UnrelatedType5 obj)
        {
            return 2.5f;
        }
    }

    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType6
    {
        public static int StaticValue(object v)
        {
            return v.ToString().Length;
        }

        public int OnlyGet { get { return 1; } }
        public int OnlySet { set { } }
        public int GetSet { get; set; }
    }


    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType7
    {
        protected bool Equals(UnrelatedType7 other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UnrelatedType7) obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public int Value { get; set; }

        public static int operator +(UnrelatedType7 a, int b)
        {
            return a.Value + b;
        }
        public static int operator +(int a, UnrelatedType7 b)
        {
            return a + b.Value;
        }

        public static int operator -(UnrelatedType7 a, int b)
        {
            return a.Value - b;
        }
        public static int operator -(int a, UnrelatedType7 b)
        {
            return a - b.Value;
        }

        public static int operator *(UnrelatedType7 a, int b)
        {
            return a.Value * b;
        }

        public static int operator /(UnrelatedType7 a, int b)
        {
            return a.Value / b;
        }

        public static int operator %(UnrelatedType7 a, int b)
        {
            return a.Value % b;
        }

        public static bool operator ==(UnrelatedType7 a, int b)
        {
            return a.Value == b;
        }

        public static bool operator !=(UnrelatedType7 a, int b)
        {
            return !(a == b);
        }
    }
    
    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType8
    {
        public string GetGeneric<T>(T obj)
            where T : class
        {
            return obj.ToString();
        }

        public string GetGeneric<T1, T2>(T1 obj1, int a, T2 obj2)
        {
            return obj1.ToString() + obj2.ToString();
        }

        public static explicit operator int(UnrelatedType8 a)
        {
            return 95;
        }

        public static bool operator <(UnrelatedType8 a, int b)
        {
            return true;
        }
        public static bool operator <=(UnrelatedType8 a, int b)
        {
            return true;
        }

        public static bool operator >(UnrelatedType8 a, int b)
        {
            return false;
        }
        public static bool operator >=(UnrelatedType8 a, int b)
        {
            return false;
        }

        public static bool operator &(UnrelatedType8 a, int b)
        {
            return true;
        }

        public static bool operator |(UnrelatedType8 a, int b)
        {
            return false;
        }

        public static bool operator ^(UnrelatedType8 a, int b)
        {
            return false;
        }
    }

    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType9
    {
        public string VariantMethod(object a)
        {
            return a.ToString();
        }
    }

    /// <summary>
    /// Test type.
    /// </summary>
    public struct UnrelatedType10
    {
        public int VariantMethod(object a)
        {
            return a.ToString().Length;
        }
    }

    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType11
    {
        public static string operator +(UnrelatedType11 a1, object a2)
        {
            return a2.ToString();
        }
        public static string operator +(object a1, UnrelatedType11 a2)
        {
            return a1.ToString();
        }
    }

    /// <summary>
    /// Test type.
    /// </summary>
    public struct UnrelatedType12
    {
        public static int operator +(UnrelatedType12 a1, object a2)
        {
            return a2.ToString().Length;
        }
        public static int operator +(object a1, UnrelatedType12 a2)
        {
            return a1.ToString().Length;
        }
    }

    /// <summary>
    /// Test type.
    /// </summary>
    public class MyClass
    {
        public string Method(object arg)
        {
            return arg.ToString();
        }
    }
}
