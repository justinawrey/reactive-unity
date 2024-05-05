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
        public T? Value => _val;

        protected virtual bool EqualityFunc(T? first, T? second) => Equals(first, second);

        private List<Action<T?, T?>> _cbs = new List<Action<T?, T?>>();

        protected void Set(T? to)
        {
            T? prevValue = _val;
            _val = to;

            if (EqualityFunc(prevValue, _val))
            {
                return;
            }

            _cbs.ForEach(cb => cb(prevValue, _val));
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
