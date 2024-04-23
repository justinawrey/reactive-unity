using System;
using UnityEngine;

namespace ReactiveUnity
{
    [Serializable]
    public class Computed<TOut> : ReactiveBase<TOut>
    {
        private Action _unsub = null;

        public Action React<TIn1>(IReactiveCallbackOwner<TIn1> tracked, Func<TIn1, TOut> cb)
        {
            Set(cb(tracked.Value));
            return CacheUnsubCb(tracked.OnChange(curr => Set(cb(curr))));
        }

        private Action CacheUnsubCb(Action unsub)
        {
            if (_unsub != null)
            {
                _unsub();
                Debug.LogWarning(
                    "reactive-unity (computed): computed variable unsubscribe hooks have been overwritten.  Did you call 'React' twice?"
                );
            }
            _unsub = unsub;
            return _unsub;
        }

        public Action React<TIn1, TIn2>(
            IReactiveCallbackOwner<TIn1> tracked1,
            IReactiveCallbackOwner<TIn2> tracked2,
            Func<TIn1, TIn2, TOut> cb
        )
        {
            Set(cb(tracked1.Value, tracked2.Value));

            Action unsub1 = tracked1.OnChange(curr => Set(cb(curr, tracked2.Value)));
            Action unsub2 = tracked2.OnChange(curr => Set(cb(tracked1.Value, curr)));

            return CacheUnsubCb(() =>
            {
                unsub1();
                unsub2();
            });
        }

        public Action React<TIn1, TIn2, TIn3>(
            IReactiveCallbackOwner<TIn1> tracked1,
            IReactiveCallbackOwner<TIn2> tracked2,
            IReactiveCallbackOwner<TIn3> tracked3,
            Func<TIn1, TIn2, TIn3, TOut> cb
        )
        {
            Set(cb(tracked1.Value, tracked2.Value, tracked3.Value));

            Action unsub1 = tracked1.OnChange(curr =>
                Set(cb(curr, tracked2.Value, tracked3.Value))
            );
            Action unsub2 = tracked2.OnChange(curr =>
                Set(cb(tracked1.Value, curr, tracked3.Value))
            );
            Action unsub3 = tracked3.OnChange(curr =>
                Set(cb(tracked1.Value, tracked2.Value, curr))
            );

            return CacheUnsubCb(() =>
            {
                unsub1();
                unsub2();
                unsub3();
            });
        }

        public Action React<TIn1, TIn2, TIn3, TIn4>(
            IReactiveCallbackOwner<TIn1> tracked1,
            IReactiveCallbackOwner<TIn2> tracked2,
            IReactiveCallbackOwner<TIn3> tracked3,
            IReactiveCallbackOwner<TIn4> tracked4,
            Func<TIn1, TIn2, TIn3, TIn4, TOut> cb
        )
        {
            Set(cb(tracked1.Value, tracked2.Value, tracked3.Value, tracked4.Value));

            Action unsub1 = tracked1.OnChange(curr =>
                Set(cb(curr, tracked2.Value, tracked3.Value, tracked4.Value))
            );
            Action unsub2 = tracked2.OnChange(curr =>
                Set(cb(tracked1.Value, curr, tracked3.Value, tracked4.Value))
            );
            Action unsub3 = tracked3.OnChange(curr =>
                Set(cb(tracked1.Value, tracked2.Value, curr, tracked4.Value))
            );
            Action unsub4 = tracked4.OnChange(curr =>
                Set(cb(tracked1.Value, tracked2.Value, tracked3.Value, curr))
            );

            return CacheUnsubCb(() =>
            {
                unsub1();
                unsub2();
                unsub3();
                unsub4();
            });
        }
    }
}