using NQRW.Maths;
using NQRW.Robotics;
using System.Collections.Generic;

namespace NQRW.Kinematics
{
    public interface IKinematicEngine
    {
        Angle BodyRoll { get; set; }
        Angle BodyPitch { get; set; }
        Angle BodyYaw { get; set; }

        double BodyX { get; set; }
        double BodyY { get; set; }
        double BodyZ { get; set; }

        Matrix4 BodyPosition { get; }

        void SetOffsets(Dictionary<Leg, Vector3> offsets);
        Dictionary<Leg, ILeg> Legs { get; set; }

        void Update();
    }
}
