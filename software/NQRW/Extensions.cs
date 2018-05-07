using System;
using System.Reactive.Linq;

namespace NQRW
{
    public static class Extensions
    {
        public static IObservable<T> SubSample<T>(this IObservable<T> source, int interval)
        {
            return source.Where((o, i) => i % interval == 0);
        }
    }
}
