using System;
using System.Collections.Generic;

namespace ReactiveUnity
{
  public class ReactiveList<T> : List<T>
  {
    private List<Action<T>> _addCbs = new List<Action<T>>();
    private List<Action<T>> _removeCbs = new List<Action<T>>();

    public Action OnAdd(Action<T> cb)
    {
      _addCbs.Add(cb);
      return () => { _addCbs.Remove(cb); };
    }

    public Action OnRemove(Action<T> cb)
    {
      _removeCbs.Add(cb);
      return () => { _removeCbs.Remove(cb); };
    }

    public new void Add(T item)
    {
      base.Add(item);

      foreach (Action<T> cb in _addCbs)
      {
        cb(item);
      }
    }

    public new void Remove(T item)
    {
      base.Remove(item);

      foreach (Action<T> cb in _removeCbs)
      {
        cb(item);
      }
    }
  }
}