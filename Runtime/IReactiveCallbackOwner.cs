#nullable enable
using System;

namespace ReactiveUnity
{
  public interface IReactiveCallbackOwner<T>
  {
    public Action OnChange(Action<T?> cb);
    public T? Value { get; }
  }
}

#nullable disable
