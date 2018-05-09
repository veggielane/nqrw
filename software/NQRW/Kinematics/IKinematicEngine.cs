using NQRW.Maths;
using System.Collections.Generic;

namespace NQRW.Kinematics
{
    public interface IKinematicEngine
    {
        Angle Roll { get; }
        Angle Pitch { get; }
        Angle Yaw { get; }

        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        Matrix4 Position { get; }
        IList<ILeg> Legs { get; }
        void Update();
    }
}
