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
    /// Contract with parameterless method with a return value.
    /// </summary>
    public interface IIntMethod
    {
        int Value();
    }

    /// <summary>
    /// Contract with parameterless method without a return value.
    /// </summary>
    public interface IVoidMethod
    {
        void Value();
    }

    /// <summary>
    /// Contract with parameterized method.
    /// </summary>
    public interface IParameterizedMethod
    {
        int Value(int a, int b, string c, int d, int e, string f);
    }

    /// <summary>
    /// Contract with two methods.
    /// </summary>
    public interface ITwoMethods
    {
        int Value(int a, int b, string c, int d, int e, string f);
        void Value();
    }

    /// <summary>
    /// Contract with getter.
    /// </summary>
    public interface IGetter
    {
        int OnlyGet { get; }
    }

    /// <summary>
    /// Contract with setter.
    /// </summary>
    public interface ISetter
    {
        int OnlySet { set; }
    }

    /// <summary>
    /// Contract with getter and setter.
    /// </summary>
    public interface IGetterAndSetter
    {
        int GetSet { get; set; }
    }

    /// <summary>
    /// Contract with unary generic method.
    /// </summary>
    public interface IGenericMethod1
    {
        string GetGeneric<T>(T obj)
            where T : class;
    }

    /// <summary>
    /// Contract with binary generic method and mixed parameters.
    /// </summary>
    public interface IGenericMethod2
    {
        string GetGeneric<T1, T2>(T1 obj1, int a, T2 obj2);
    }

    /// <summary>
    /// Contract with explicit operator.
    /// </summary>
    public interface ICastableToInt32
    {
        [Cast]
        int ToInt32();
    }

    /// <summary>
    /// Contract with implicit operator.
    /// </summary>
    public interface IConvertibleToFloat
    {
        [Implicit]
        float ToFloat();
    }

    /// <summary>
    /// Contract with operator+ (left side).
    /// </summary>
    public interface IAddableLeft
    {
        [Addition(OpSide.ThisLeft)]
        int Add(int p);
    }

    /// <summary>
    /// Contract with operator+ (right side).
    /// </summary>
    public interface IAddableRight
    {
        [Addition(OpSide.ThisRight)]
        int Add(int p);
    }

    /// <summary>
    /// Contract with operator- (both sides).
    /// </summary>
    public interface ISubtractable
    {
        [Subtraction(OpSide.ThisLeft)]
        int Subtract(int p);

        [Subtraction(OpSide.ThisRight)]
        int SubtractFrom(int p);
    }

    /// <summary>
    /// Generic contract with operator* (left side).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMultipliable<T>
    {
        [Multiplication(OpSide.ThisLeft)]
        T MultiplyBy(T p);
    }

    /// <summary>
    /// Generic contract with operator/ (left side).
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public interface IDividable<out T1, in T2>
    {
        [Division(OpSide.ThisLeft)]
        T1 DivideBy(T2 p);
    }

    /// <summary>
    /// Contract with operator% (left side).
    /// </summary>
    public interface IModulusOperator
    {
        [Modulus(OpSide.ThisLeft)]
        int Modulus(int p);
    }

    /// <summary>
    /// Generic contract with operator== and operator!= (left side).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IComparableTo1<T>
    {
        [Equality(OpSide.ThisLeft)]
        bool Equals(T p);

        [Inequality(OpSide.ThisLeft)]
        bool NotEqualTo(T p);
    }

    /// <summary>
    /// Generic contract with operator greater-than (left side).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IComparableTo2<T>
    {
        [GreaterThan(OpSide.ThisLeft)]
        bool GreaterThan(T op);
    }

    /// <summary>
    /// Generic contract with operator less-than (left side).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IComparableTo3<T>
    {
        [LessThan(OpSide.ThisLeft)]
        bool LessThan(T op);
    }

    /// <summary>
    /// Generic contract with operator greater-than-or-equal (left side).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IComparableTo4<T>
    {
        [GreaterThanOrEqual(OpSide.ThisLeft)]
        bool GreaterThanOrEqual(T op);
    }

    /// <summary>
    /// Generic contract with operator less-than-or-equal (left side).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IComparableTo5<T>
    {
        [LessThanOrEqual(OpSide.ThisLeft)]
        bool LessThanOrEqual(T op);
    }
    
    /// <summary>
    /// Contract inheriting other contract.
    /// </summary>
    public interface ICastableToInt : ICastableTo<int>
    {
    }

}
