#nullable enable
using System;
using System.Collections.Generic;

namespace ReactiveUnity
{
    [Serializable]
    public class ComputedSet<T> : Computed<HashSet<T>>
        where T : IEquatable<T>
    {
        protected override bool EqualityFunc(HashSet<T>? first, HashSet<T>? second)
        {
            if (first is null && second is null)
            {
                return true;
            }

            if (first is null && second is not null)
            {
                return false;
            }

            if (first is not null && second is null)
            {
                return false;
            }

            if (first is HashSet<T> f && second is HashSet<T> s)
            {
                return f.SetEquals(s);
            }

            return false;
        }
    }
}
#nullable disable
