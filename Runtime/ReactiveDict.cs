using System;
using System.Collections.Generic;

namespace ReactiveUnity
{
  public class ReactiveDict<K, V> : Dictionary<K, V>
  {
    private List<Action<K, V>> _addCbs = new List<Action<K, V>>();
    private List<Action<K, V>> _removeCbs = new List<Action<K, V>>();

    public Action OnAdd(Action<K, V> cb)
    {
      _addCbs.Add(cb);
      return () => { _addCbs.Remove(cb); };
    }

    public Action OnRemove(Action<K, V> cb)
    {
      _removeCbs.Add(cb);
      return () => { _removeCbs.Remove(cb); };
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
  }
}