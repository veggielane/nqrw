using NQRW.Maths;

namespace NQRW.Kinematics
{
    public class Body : IBody
    {
        public Matrix4 Position => Matrix4.Translate(X, Y, Z) * Matrix4.RotateZ(Yaw) * Matrix4.RotateY(Pitch) * Matrix4.RotateX(Roll);

        public Angle Roll { get; set; } = Angle.FromDegrees(0);

        public Angle Pitch { get; set; } = Angle.FromDegrees(0);

        public Angle Yaw { get; set; } = Angle.FromDegrees(0);

        public double X { get; set; } = 0;

        public double Y { get; set; } = 0;

        public double Z { get; set; } = 0;
    }
}
