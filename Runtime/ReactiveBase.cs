#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveUnity
{
    [Serializable]
    public class ReactiveBase<T> : IReactiveCallbackOwner<T>
    {
        [SerializeField]
        protected T? _val;
        public T? Value
        {
            get { return _val; }
        }

        protected virtual bool EqualityFunc(T? first, T? second)
        {
            if (first is null && second is null)
            {
                return true;
            }

            if (first is null && second is not null)
            {
                return false;
            }

            if (first is not null && second is null)
            {
                return false;
            }

            // try and avoid the implicit boxing conversion
            if (first is IEquatable<T> f && second is IEquatable<T> s)
            {
                return f.Equals(s);
            }

            return Equals(first, second);
        }

        protected virtual void FlushAdditionalCbs(T? prev, T? curr) { }

        private List<Action<T?, T?>> _cbs = new List<Action<T?, T?>>();

        protected void Set(T? to)
        {
            T? prevValue = _val;
            _val = to;

            if (EqualityFunc(prevValue, _val))
            {
                return;
            }

            int numCbs = _cbs.Count;
            for (int i = 0; i < numCbs; i++)
            {
                _cbs[i](prevValue, _val);
            }
            FlushAdditionalCbs(prevValue, _val);
        }

        public Action OnChange(Action<T?, T?> cb)
        {
            _cbs.Add(cb);
            return () => _cbs.Remove(cb);
        }

        public Action OnChange(Action<T?> cb)
        {
            Action<T?, T?> curried = (prev, curr) => cb(curr);
            _cbs.Add(curried);
            return () => _cbs.Remove(curried);
        }
    }
}
#nullable disable
