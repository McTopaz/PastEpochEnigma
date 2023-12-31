﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Enums;

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

        internal static void InsertListInList<T>(List<T> source, List<T> destination)
        {
            var rnd = new Random();
            foreach (var item in source)
            {
                var max = destination.Count() + 1;
                var index = rnd.Next(max);
                destination.Insert(index, item);
            }
        }

        internal static T GetEnum<T>()
        {
            var type = typeof(T);
            var values = Enum.GetValues(type);
            var index = new Random().Next(values.Length);
            var value = values.GetValue(index);
            return (T)value;
        }

        internal static T GetRandomInList<T>(List<T> list)
        {
            var rnd = new Random();
            var index = rnd.Next(list.Count);
            return list[index];
        }

        internal static Side GetRandomDirection(List<Side> forbiddenDirections) 
        {
            var values = Enum.GetValues(typeof(Side))
                .Cast<Side>()
                .Except(forbiddenDirections)
                .ToList();
            var index = new Random().Next(values.Count());
            return values[index];
        }
    }
}
