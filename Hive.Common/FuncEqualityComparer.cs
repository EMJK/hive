using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hive.Common
{
    public class FuncEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equals;
        private readonly Func<T, int> _getHashCode;

        public FuncEqualityComparer(Func<T, T, bool> comparer, Func<T, int> getHashCode)
        {
            _equals = comparer;
            _getHashCode = getHashCode;
        }

        public FuncEqualityComparer(Func<T, T, bool> comparer)
        {
            _equals = comparer;
            _getHashCode = item => item.GetHashCode();
        }

        public bool Equals(T x, T y)
        {
            return _equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _getHashCode(obj);
        }
    }
}
