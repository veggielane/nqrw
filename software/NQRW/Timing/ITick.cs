using System;
using System.Collections.Generic;
using System.Text;

namespace NQRW.Timing
{
    public interface ITick
    {

        TimeSpan TimeElapsed { get; }
        TimeSpan TimeDelta { get; }
        long TickCount { get; }

        void Update(TimeSpan delta);
    }
}
