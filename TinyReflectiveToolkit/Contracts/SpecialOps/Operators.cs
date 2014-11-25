using System;

namespace TinyReflectiveToolkit.Contracts.SpecialOps
{
    public static partial class SpecialOperations
    {
        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static string Concat(string str1, string str2)
        {
            return string.Concat(str1, str2);
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static string Concat(int str1, string str2)
        {
            return string.Concat(str1, str2);
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static string Concat(string str1, int str2)
        {
            return string.Concat(str1, str2);
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static string Concat(object str1, string str2)
        {
            return string.Concat(str1, str2);
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static string Concat(string str1, object str2)
        {
            return string.Concat(str1, str2);
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static int Add(int a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static object AddObject(int a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static float Add(float a, float b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static float Add(int a, float b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static float Add(float a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static double Add(double a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static double Add(int a, double b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static int Add(int a, byte b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static int Add(byte a, int b)
        {
            return a + b;
        }
    }
}
