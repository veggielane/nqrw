using NQRW.Maths;
using NQRW.Robotics;
using System.Collections.Generic;
using NQRW.Kinematics;

namespace NQRW.Gait
{
    public interface IGaitEngine
    {
        double StrideLength { get; }
        Angle StrideAngle { get; }
        double StrideHeight { get; }
        Vector2 Heading { get; set; }
        Angle Rotation { get; set; }
        IDictionary<Leg, Vector3> Offsets { get; }

        WalkMode Mode { get; set; }
        void Update(Dictionary<Leg, ILeg> legs);
        //void Start();
        void Stop();
    }
}
