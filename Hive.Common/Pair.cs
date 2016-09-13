using System;

namespace Hive.Common
{
    public class Pair<T1, T2> : IEquatable<Pair<T1, T2>>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public bool Equals(Pair<T1, T2> other)
        {
            if (other == null) return false;
            return Equals(Item1, other.Item1) && Equals(Item2, other.Item2);
        }

        public override string ToString()
        {
            return $"{Item1}, {Item2}";
        }

        public override bool Equals(object obj)
        {
            var obj2 = obj as Pair<T1, T2>;
            if (obj2 != null)
                return Equals(obj2);
            return false;
        }

        public override int GetHashCode()
        {
            return
                typeof(T1).GetHashCode() ^
                typeof(T2).GetHashCode() ^
                Item1?.GetHashCode() ?? 0 ^
                Item2?.GetHashCode() ?? 0;
        }
    }

    public static class Pair
    {
        public static Pair<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Pair<T1, T2>
            {
                Item1 = item1,
                Item2 = item2
            };
        }
    }
}