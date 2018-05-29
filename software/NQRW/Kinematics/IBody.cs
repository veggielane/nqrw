using NQRW.Maths;

namespace NQRW.Kinematics
{
    public interface IBody
    {
        Angle Roll { get; set; }
        Angle Pitch { get; set; }
        Angle Yaw { get; set; }

        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        Matrix4 Position { get; }
    }
}
