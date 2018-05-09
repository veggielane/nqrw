using NQRW.Maths;
using NQRW.Robotics;
using System.Collections.Generic;

namespace NQRW.Kinematics
{
    public interface IKinematicEngine
    {
        Angle BodyRoll { get; }
        Angle BodyPitch { get; }
        Angle BodyYaw { get; }

        double BodyX { get; set; }
        double BodyY { get; set; }
        double BodyZ { get; set; }

        Matrix4 BodyPosition { get; }

        Dictionary<Leg, ILeg> Legs { get; set; }

        void Update();
    }
}
