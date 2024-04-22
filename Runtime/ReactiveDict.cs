#nullable enable
using System;
using System.Collections.Generic;

namespace ReactiveUnity
{
  [Serializable]
  public class ReactiveDict<K, V> : Dictionary<K, V>, IReactiveCallbackOwner<Dictionary<K, V>>
  {
    private List<Action<K, V>> _addCbs = new List<Action<K, V>>();
    private List<Action<K, V>> _removeCbs = new List<Action<K, V>>();
    public Dictionary<K, V>? Value => this;

    public Action OnAdd(Action<K, V> cb)
    {
      _addCbs.Add(cb);
      return () => _addCbs.Remove(cb);
    }

    public Action OnRemove(Action<K, V> cb)
    {
      _removeCbs.Add(cb);
      return () => _removeCbs.Remove(cb);
    }

    public Action OnChange(Action<Dictionary<K, V>?> cb)
    {
      Action<K, V> curried = (_, __) => cb(Value);

      _addCbs.Add(curried);
      _removeCbs.Add(curried);

      return () =>
      {
        _addCbs.Remove(curried);
        _removeCbs.Remove(curried);
      };
    }

    public new void Add(K key, V val)
    {
      base.Add(key, val);

      foreach (Action<K, V> cb in _addCbs)
      {
        cb(key, val);
      }
    }

    public new void Remove(K key)
    {
      V val = this[key];
      base.Remove(key);

      foreach (Action<K, V> cb in _removeCbs)
      {
        cb(key, val);
      }
    }

    public new void Clear()
    {
      foreach (var kvp in this)
      {
        foreach (Action<K, V> cb in _removeCbs)
        {
          cb(kvp.Key, kvp.Value);
        }
      }

      base.Clear();
    }
  }
}
#nullable disable
