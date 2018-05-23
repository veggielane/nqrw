using System;
using System.Collections.Generic;
using System.Text;
using NQRW.Maths;

namespace NQRW.Kinematics
{
    public class Leg4DOF : ILeg
    {
        public Matrix4 BasePosition { get; private set; }
        public Vector3 FootPosition { get; set; }
        public Vector3 FootOffset { get; set; }

        public Leg4DOFConstraint Constraint { get; set; } = Leg4DOFConstraint.NormalToGround;


        public Leg4DOF(Matrix4 basePosition, Vector3 footPosition, double coxaLength, double femurLength=76, double tibiaLength=76, double tarsusLength=90)
        {
            BasePosition = basePosition;
            FootPosition = footPosition;
            CoxaLength = coxaLength;
            FemurLength = femurLength;
            TibiaLength = tibiaLength;
            TarsusLength = tarsusLength;
            FootOffset = Vector3.Zero;
        }
        //Geometry
        public double CoxaLength { get; set; }
        public double FemurLength { get; set; }
        public double TibiaLength { get; set; }
        public double TarsusLength { get; set; }


        public Angle CoxaOffset { get; set; }
        public Angle FemurOffset { get; set; }
        public Angle TibiaOffset { get; set; }
        public Angle TarsusOffset { get; set; }


        public double Distance { get; set; } = 0;
        public Angle Angle1 { get; set; } = Angle.Zero;
        public Angle Angle2 { get; set; } = Angle.Zero;
        public Angle Angle3 { get; set; } = Angle.Zero;
        public Angle Angle4 { get; set; } = Angle.Zero;

        public void Update(Matrix4 bodyPosition)
        {
            var basePos = bodyPosition * BasePosition;
            var baseToFoot = (FootPosition + FootOffset) - basePos.ToVector3();
            var foot = basePos.RotationComponent.Inverse() * baseToFoot.ToMatrix4();
            Distance = baseToFoot.Length;

            var z_dash = -foot.Z;
            var x_dash = Math.Sqrt(Math.Pow(foot.X, 2) + Math.Pow(foot.Y, 2));

            var a = FemurLength;
            var b = TibiaLength;
            var c = TarsusLength;
            var d = Math.Sqrt(Math.Pow(x_dash - CoxaLength, 2) + Math.Pow(z_dash, 2));
            var theta_d = Angle.FromRadians(Math.Acos(z_dash / d));
            var theta_b = Angle.FromRadians(Math.Acos((d * d + c * c - 2 * d * c * theta_d.Cos() - b * b - a * a) / (-2 * b * a)));
            var e = Math.Sqrt(d * d + c * c - 2 * d * c * theta_d.Cos());
            var theta_a1 = Angle.FromRadians(Math.Acos((a * a + e * e - b * b) / (2 * a * e)));
            var theta_a2 = Angle.FromRadians(Math.Acos((e * e + d * d - c * c) / (2 * e * d)));
            var theta_a = theta_a1 + theta_a2;
            var theta_c = Angle.TwoPI - theta_a - theta_b - theta_d;

            Angle1 = Trig.Atan2(-foot.Y, foot.X);
            Angle2 = theta_d + theta_a - Angle.FromDegrees(90);
            Angle3 = Angle.PI - theta_b;
            Angle4 = Angle.PI - theta_c;
        }
    }
    public enum Leg4DOFConstraint
    {
        NormalToGround
    }
}
