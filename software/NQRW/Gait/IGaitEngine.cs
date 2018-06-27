using NQRW.Maths;
using NQRW.Robotics;
using System.Collections.Generic;

namespace NQRW.Gait
{
    public interface IGaitEngine
    {
        Vector2 Heading { get; set; }
        IDictionary<Leg, Vector3> Offsets { get; }

        bool Moving { get; }
        void Update();
        void Start();
        void Stop();
    }
}
