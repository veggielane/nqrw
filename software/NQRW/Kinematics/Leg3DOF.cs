using NQRW.Maths;

namespace NQRW.Kinematics
{
    public class Leg3DOF : ILeg
    {
        public Matrix4 BasePosition { get; private set; }
        public Vector3 FootPosition { get; set; }
        public Vector3 FootOffset { get; set; }

        public Leg3DOF(Matrix4 basePosition)
        {
            BasePosition = basePosition;
            FootPosition = Vector3.Zero;
            FootOffset = Vector3.Zero;
        }

        public bool CoxaInvert;
        public bool FemurInvert;
        public bool TibiaInvert;

        public double CoxaLength { get; set; }
        public double FemurLength { get; set; }
        public double TibiaLength { get; set; }

        public Angle CoxaOffset { get; set; }
        public Angle FemurOffset { get; set; }
        public Angle TibiaOffset { get; set; }

        //public readonly Servo CoxaServo = new Servo();
        //public readonly Servo FemurServo = new Servo();
        // public readonly Servo TibiaServo = new Servo();

        public void Inverse(IBody body)
        {
            /*
            * Todo: 
            * - Elbow Up + Elbow Down
            * - Reduce Calcs
            * 
            * Jazar Pg. 331
            */
            var LtoF = (FootPosition + FootOffset) - BasePosition.ToVector3();
            var relative = (BasePosition.RotationComponent * LtoF.ToMatrix4());
            var angle1 = Trig.Atan2(relative.Y, relative.X);
            var a = (Matrix4.RotateZ(angle1) * Matrix4.Translate(CoxaLength, 0, 0)).ToVector3();
            var c = relative.ToVector3();
            var atoc = c - a;
            var angle3 = 2 * Trig.Atan2(((FemurLength + TibiaLength).Pow(2) - (atoc.X.Pow(2) + atoc.Z.Pow(2))).Sqrt(), (atoc.X.Pow(2) + atoc.Z.Pow(2) - (FemurLength - TibiaLength).Pow(2)).Sqrt());
            var angle2 = Trig.Atan2(atoc.Z, atoc.X) + Trig.Atan2(TibiaLength * Angle.FromRadians(angle3).Sin(), FemurLength + TibiaLength * Angle.FromRadians(angle3).Cos());
            //CoxaServo.Angle = Angle.FromRadians((CoxaInvert ? -1 : 1) * (angle1 + CoxaOffset));
            //FemurServo.Angle = Angle.FromRadians((FemurInvert ? -1 : 1) * (angle2 + FemurOffset));
            //TibiaServo.Angle = Angle.FromRadians((TibiaInvert ? -1 : 1) * (angle3 + TibiaOffset));
        }
    }
}
