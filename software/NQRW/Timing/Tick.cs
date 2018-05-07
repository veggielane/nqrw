using System;

namespace NQRW.Timing
{
    public class Tick : ITick
    {
        public TimeSpan TimeElapsed { get; private set; }
        public TimeSpan TimeDelta { get; private set; }
        public long TickCount { get; private set; }
        public void Update(TimeSpan delta)
        {
            TimeDelta = delta;
            TimeElapsed += TimeDelta;
            TickCount++;
        }
    }
}
