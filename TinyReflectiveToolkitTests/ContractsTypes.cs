using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyReflectiveToolkit.Contracts;
using TinyReflectiveToolkit.Contracts.Premade;

namespace TinyReflectiveToolkitTests
{
    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType1
    {
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
        public void Value()
        {
        }
    }
    
    /// <summary>
    /// Test type.
    /// </summary>
    public class UnrelatedType5
    {
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

        public static bool operator >(UnrelatedType8 a, int b)
        {
            return false;
        }
    }


}
