using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using JetBrains.Annotations;

namespace NQRW.Timing
{
    [UsedImplicitly]
    public class Timer : ITimer
    {
        public IObservable<ITick> Ticks { get; private set; }

        public TimeSpan Delta { get; private set; }
        public Tick LastTickTime { get; private set; }

        private readonly ISubject<ITick> _subject;

        private readonly IObservable<Int64> _timer;

        public Timer()
        {
            _subject = new Subject<ITick>();
            Delta = TimeSpan.FromMilliseconds(0.5);
            _timer = Observable.Interval(Delta);
            Ticks = _subject.AsObservable();
            LastTickTime = new Tick();
        }

        private IDisposable _sub;
        public void Start()
        {
            _sub = _timer.Subscribe(t =>
            {
                _subject.OnNext(LastTickTime);
                LastTickTime.Update(Delta);
            });
        }

        public void Stop() => _sub?.Dispose();
    }
}
