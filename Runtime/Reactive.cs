using System;
using System.Collections.Generic;

namespace ReactiveUnity
{
    public class Reactive<T>
    {
        private T _val;
        private Dictionary<Func<T, T, bool>, List<Action<T, T>>> _predicates = new Dictionary<Func<T, T, bool>, List<Action<T, T>>>();

        public Reactive(T val)
        {
            _val = val;
        }

        // Convenience getter & setter
        public T Value
        {
            get
            {
                return _val;
            }
            set
            {
                Set(value);
            }
        }

        public void When(Func<T, T, bool> predicate, Action<T, T> cb)
        {
            AddPredicate(predicate, cb);
        }

        public void OnChange(Action<T, T> cb)
        {
            Func<T, T, bool> predicate = (_, __) => true;
            AddPredicate(predicate, cb);
        }

        private void Set(T to)
        {
            T prevValue = _val;
            _val = to;

            if (prevValue.Equals(_val))
            {
                return;
            }

            foreach (var pair in _predicates)
            {
                Func<T, T, bool> predicate = pair.Key;
                List<Action<T, T>> cbs = pair.Value;

                if (!predicate(prevValue, _val))
                {
                    continue;
                }

                cbs.ForEach(cb => cb(prevValue, _val));
            }
        }

        private void AddPredicate(Func<T, T, bool> predicate, Action<T, T> cb)
        {
            if (_predicates.ContainsKey(predicate))
            {
                _predicates[predicate].Add(cb);
            }
            else
            {
                _predicates[predicate] = new List<Action<T, T>>() { cb };
            }
        }
    }
}