<<<<<<< HEAD
#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveUnity
{
    [Serializable]
    public class Reactive<T>
    {
        [SerializeField] private T? _val;
        private Dictionary<Func<T?, T?, bool>, List<Action<T?, T?>>> _predicates = new Dictionary<Func<T?, T?, bool>, List<Action<T?, T?>>>();

        public Reactive(T? val)
        {
            _val = val;
        }

        // Convenience getter & setter
        public T? Value
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

        public Action When(Func<T?, T?, bool> predicate, Action<T?, T?> cb)
        {
            AddCb(predicate, cb);
            return () => RemoveCb(predicate, cb);
        }

        public Action OnChange(Action<T?, T?> cb)
        {
            Func<T?, T?, bool> predicate = (_, __) => true;
            AddCb(predicate, cb);
            return () => RemoveCb(predicate, cb);
        }

        private void Set(T? to)
        {
            T? prevValue = _val;
            _val = to;
            
            if (Equals(prevValue, _val)) 
            {
                return;
            }

            foreach (var pair in _predicates)
            {
                Func<T?, T?, bool> predicate = pair.Key;
                List<Action<T?, T?>> cbs = pair.Value;

                if (!predicate(prevValue, _val))
                {
                    continue;
                }

                cbs.ForEach(cb => cb(prevValue, _val));
            }
        }

        private void AddCb(Func<T?, T?, bool> predicate, Action<T?, T?> cb)
        {
            if (_predicates.ContainsKey(predicate))
            {
                _predicates[predicate].Add(cb);
            }
            else
            {
                _predicates[predicate] = new List<Action<T?, T?>>() { cb };
            }
        }

        private void RemoveCb(Func<T?, T?, bool> predicate, Action<T?, T?> cb)
        {
            if (!_predicates.ContainsKey(predicate))
            {
                return;
            }
            
            _predicates[predicate].Remove(cb);
            if(_predicates[predicate].Count == 0)
            {
                _predicates.Remove(predicate);      
            }               
        }
    }
}
#nullable disable
=======
using System;

namespace ReactiveUnity
{
  [Serializable]
  public class Reactive<T> : ReactiveBase<T>
  {
    public Reactive(T val)
    {
      _val = val;
    }

    public new T Value
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
  }
}
>>>>>>> 1a1ea1f (update to 2.0.0)
