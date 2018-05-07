using NQRW.Maths;
using NQRW.Robotics;
using System.Collections.Generic;

namespace NQRW.Gait
{
    public interface IGaitEngine
    {
        Dictionary<Leg, Vector3> Run(Vector2 heading);
    }
}
