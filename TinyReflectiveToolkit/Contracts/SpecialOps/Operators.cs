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
        public static int Add(byte a, byte b)
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
        public static int Add(char a, char b)
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
        public static double Add(double a, double b)
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
        public static int Add(byte a, char b)
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
        public static int Add(char a, byte b)
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
        public static float Add(byte a, float b)
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
        public static float Add(float a, byte b)
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
        public static double Add(byte a, double b)
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
        public static double Add(double a, byte b)
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
        public static int Add(char a, int b)
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
        public static int Add(int a, char b)
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
        public static double Add(float a, double b)
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
        public static double Add(double a, float b)
        {
            return a + b;
        }

        private static bool Equal(double a, double b)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return a == b;
        }

        private static bool Equal(int a, int b)
        {
            return a == b;
        }
    }
}
