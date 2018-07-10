using NQRW.Maths;

namespace NQRW.Kinematics
{
    public class Body : IBody
    {
        public Matrix4 Position => Matrix4.Translate(X, Y, Z) * Matrix4.RotateZ(Yaw) * Matrix4.RotateY(Pitch) * Matrix4.RotateX(Roll);

        //public Matrix4 Position => Matrix4.RotateX(Roll) * Matrix4.RotateY(Pitch) * Matrix4.RotateZ(Yaw)* Matrix4.Translate(X, Y, Z);

        public Angle Roll { get; set; } = Angle.FromDegrees(0);

        public Angle Pitch { get; set; } = Angle.FromDegrees(0);

        public Angle Yaw { get; set; } = Angle.FromDegrees(0);

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public void Reset(double z)
        {
            X = 0;
            Y = 0;
            Z = z;
            Roll = 0;
            Pitch = 0;
            Yaw = 0;
        }
    }
}
