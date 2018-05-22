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

            Angle1 = Trig.Atan2(-foot.Y, foot.X);
            var l = Math.Sqrt(Math.Pow(Math.Sqrt(Math.Pow(foot.X, 2) + Math.Pow(foot.Y, 2)) - CoxaLength, 2) + Math.Pow(foot.Z, 2));
            var theta_d = Angle.FromRadians(Math.Acos(-foot.Z / l));
            var theta_b = Angle.FromRadians(Math.Acos((l * l + TarsusLength * TarsusLength - 2 * l * TarsusLength * theta_d.Cos() - TibiaLength * TibiaLength - FemurLength * FemurLength) / (-2 * TibiaLength * FemurLength)));


            //Math.Acos((l * TarsusLength * theta_d.Cos() - (l * l + TarsusLength * TarsusLength - TibiaLength * TibiaLength - FemurLength * FemurLength) / 2.0) / TibiaLength * FemurLength)

            Angle3 = Angle.PI - theta_b;




            //var A = new Vector3(Angle1.Cos() * CoxaLength, - Angle1.Sin() * CoxaLength, 0);
            //var AtoD = A - foot.ToVector3();

            //var d = AtoD.Length;




            //var z = -foot.Z;
            // var x = Trig.Atan2(AtoD.Y, AtoD.X);



            //var lengthWithOffset = 0;

            //var relative = (basePos.RotationComponent * (baseToFoot.ToMatrix4()));


            //var y = Angle.FromRadians(tt.DotProduct(-Vector2.UnitX));



        }
    }
    public enum Leg4DOFConstraint
    {
        NormalToGround
    }
}
