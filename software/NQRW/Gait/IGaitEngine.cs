using NQRW.Maths;
using NQRW.Robotics;
using System.Collections.Generic;

namespace NQRW.Gait
{
    public interface IGaitEngine
    {
        Vector2 Heading { get; set; }

        bool Moving { get; set; }
        Dictionary<Leg, Vector3> Update();
    }
}
