using System;
using System.Collections.Generic;

namespace ReactiveUnity
{
    public class Reactive<T>
    {
        private T _val;
        private Dictionary<Func<T, bool>, List<Action<T>>> _predicates = new Dictionary<Func<T, bool>, List<Action<T>>>();

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

        public void When(Func<T, bool> predicate, Action<T> cb)
        {
            AddPredicate(predicate, cb);
        }

        public void OnChange(Action<T> cb)
        {
            Func<T, bool> predicate = val => true;
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
                Func<T, bool> predicate = pair.Key;
                List<Action<T>> cbs = pair.Value;

                if (!predicate(_val))
                {
                    continue;
                }

                cbs.ForEach(cb => cb(_val));
            }
        }

        private void AddPredicate(Func<T, bool> predicate, Action<T> cb)
        {
            if (_predicates.ContainsKey(predicate))
            {
                _predicates[predicate].Add(cb);
            }
            else
            {
                _predicates[predicate] = new List<Action<T>>() { cb };
            }
        }
    }
}