using System;

namespace ReactiveUnity
{
  [Serializable]
  public class Computed<TOut, TIn> : ReactiveBase<TOut>
  {
    private readonly IReactiveCallbackOwner<TIn> _tracked;

    public Computed(IReactiveCallbackOwner<TIn> tracked)
    {
      _tracked = tracked;
    }

    public Action React(Func<TIn, TOut> cb)
    {
      Set(cb(_tracked.Value));
      return _tracked.OnChange(curr => Set(cb(curr)));
    }
  }

  [Serializable]
  public class Computed<TOut, TIn1, TIn2> : ReactiveBase<TOut>
  {
    private readonly IReactiveCallbackOwner<TIn1> _tracked1;
    private readonly IReactiveCallbackOwner<TIn2> _tracked2;

    public Computed(IReactiveCallbackOwner<TIn1> tracked1, IReactiveCallbackOwner<TIn2> tracked2)
    {
      _tracked1 = tracked1;
      _tracked2 = tracked2;
    }

    public Action React(Func<TIn1, TIn2, TOut> cb)
    {
      Set(cb(_tracked1.Value, _tracked2.Value));

      Action unsub1 = _tracked1.OnChange(curr => Set(cb(curr, _tracked2.Value)));
      Action unsub2 = _tracked2.OnChange(curr => Set(cb(_tracked1.Value, curr)));

      return () =>
      {
        unsub1();
        unsub2();
      };
    }
  }

  [Serializable]
  public class Computed<TOut, TIn1, TIn2, TIn3> : ReactiveBase<TOut>
  {
    private readonly IReactiveCallbackOwner<TIn1> _tracked1;
    private readonly IReactiveCallbackOwner<TIn2> _tracked2;
    private readonly IReactiveCallbackOwner<TIn3> _tracked3;

    public Computed(
      IReactiveCallbackOwner<TIn1> tracked1,
      IReactiveCallbackOwner<TIn2> tracked2,
      IReactiveCallbackOwner<TIn3> tracked3
    )
    {
      _tracked1 = tracked1;
      _tracked2 = tracked2;
      _tracked3 = tracked3;
    }

    public Action React(Func<TIn1, TIn2, TIn3, TOut> cb)
    {
      Set(cb(_tracked1.Value, _tracked2.Value, _tracked3.Value));

      Action unsub1 = _tracked1.OnChange(curr => Set(cb(curr, _tracked2.Value, _tracked3.Value)));
      Action unsub2 = _tracked2.OnChange(curr => Set(cb(_tracked1.Value, curr, _tracked3.Value)));
      Action unsub3 = _tracked3.OnChange(curr => Set(cb(_tracked1.Value, _tracked2.Value, curr)));

      return () =>
      {
        unsub1();
        unsub2();
        unsub3();
      };
    }
  }

  [Serializable]
  public class Computed<TOut, TIn1, TIn2, TIn3, TIn4> : ReactiveBase<TOut>
  {
    private readonly IReactiveCallbackOwner<TIn1> _tracked1;
    private readonly IReactiveCallbackOwner<TIn2> _tracked2;
    private readonly IReactiveCallbackOwner<TIn3> _tracked3;
    private readonly IReactiveCallbackOwner<TIn4> _tracked4;

    public Computed(
      IReactiveCallbackOwner<TIn1> tracked1,
      IReactiveCallbackOwner<TIn2> tracked2,
      IReactiveCallbackOwner<TIn3> tracked3,
      IReactiveCallbackOwner<TIn4> tracked4
    )
    {
      _tracked1 = tracked1;
      _tracked2 = tracked2;
      _tracked3 = tracked3;
      _tracked4 = tracked4;
    }

    public Action React(Func<TIn1, TIn2, TIn3, TIn4, TOut> cb)
    {
      Set(cb(_tracked1.Value, _tracked2.Value, _tracked3.Value, _tracked4.Value));

      Action unsub1 = _tracked1.OnChange(curr => Set(cb(curr, _tracked2.Value, _tracked3.Value, _tracked4.Value)));
      Action unsub2 = _tracked2.OnChange(curr => Set(cb(_tracked1.Value, curr, _tracked3.Value, _tracked4.Value)));
      Action unsub3 = _tracked3.OnChange(curr => Set(cb(_tracked1.Value, _tracked2.Value, curr, _tracked4.Value)));
      Action unsub4 = _tracked4.OnChange(curr => Set(cb(_tracked1.Value, _tracked2.Value, _tracked3.Value, curr)));

      return () =>
      {
        unsub1();
        unsub2();
        unsub3();
        unsub4();
      };
    }
  }
}
