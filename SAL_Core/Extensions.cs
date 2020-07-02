using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAL_Core
{
    public static class Extensions
    {
        public static int Mod(this int x, int m)
        {
            return (x % m + m) % m;
        }

        public static void Shift<T>(this IList<T> list, T item, int count)
        {
            var len = list.Count();
            if (count == 0) return;
            else if(count > 0)
            {
                for(int i = list.IndexOf(item); i < len - 1 && count > 0; i++, count--)
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
    }

}
