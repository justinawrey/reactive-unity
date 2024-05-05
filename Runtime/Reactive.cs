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
            get { return _val; }
            set { Set(value); }
        }
    }
}
