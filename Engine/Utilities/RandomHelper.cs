using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities
{
    internal class RandomHelper
    {
        internal static List<T> ShuffleList<T>(List<T> list)
        {
            var rnd = new Random();
            return list.Select(x => new { value = x, order = rnd.Next() })
                .OrderBy(x => x.order).Select(x => x.value).ToList();
        }

        internal static Dictionary<T1, T2> ShuffleDictionary<T1, T2>(Dictionary<T1, T2> dict)
        {
            var rnd = new Random();
            var keys = dict.Keys.ToList();
            var shuffledKeys = ShuffleList<T1>(keys);
            var shuffled = shuffledKeys.ToDictionary(t1 => t1, t2 => dict[t2]);
            return shuffled;
        }
    }
}
