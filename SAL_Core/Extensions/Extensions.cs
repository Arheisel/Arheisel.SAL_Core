using System;
using System.Collections.Generic;


namespace SAL_Core.Extensions
{
    public static class Extensions
    {
        public static int Mod(this int x, int m)
        {
            return (x % m + m) % m;
        }

        public static bool Even(this int i)
        {
            return i % 2 == 0;
        }

        public static bool Odd(this int i)
        {
            return !i.Even();
        }

        public static void Shift<T>(this IList<T> list, T item, int count)
        {
            var len = list.Count;
            if (count == 0) return;
            else if (count > 0)
            {
                for (int i = list.IndexOf(item); i < len - 1 && count > 0; i++, count--)
                {
                    list.Swap(i, i + 1);
                }
            }
            else
            {
                for (int i = list.IndexOf(item); i > 0 && count < 0; i--, count++)
                {
                    list.Swap(i, i - 1);
                }
            }
        }

        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static T[] Concat<T>(this T[] x, T[] y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            int oldLen = x.Length;
            Array.Resize<T>(ref x, x.Length + y.Length);
            Array.Copy(y, 0, x, oldLen, y.Length);
            return x;
        }

        public static T[] Concat<T>(this T[] x, T y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            Array.Resize<T>(ref x, x.Length + 1);
            x[x.Length - 1] = y;
            return x;
        }

        public static T[] Splice<T>(this T[] array, int startIndex, int length)
        {
            var ret = new T[length];
            Array.Copy(array, startIndex, ret, 0, length);
            return ret;
        }

        public static void Populate<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }
    }

}
