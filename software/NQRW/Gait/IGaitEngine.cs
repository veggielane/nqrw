using NQRW.Maths;
using NQRW.Robotics;
using System.Collections.Generic;

namespace NQRW.Gait
{
    public interface IGaitEngine
    {
        double HeadingX { get; set; }
        double HeadingY { get; set; }

        bool Moving { get; set; }
        Dictionary<Leg, Vector3> Update();
    }
}
