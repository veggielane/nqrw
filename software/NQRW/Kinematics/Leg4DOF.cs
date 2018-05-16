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

        public Leg4DOF(Matrix4 basePosition, Vector3 footPosition, double coxaLength)
        {
            BasePosition = basePosition;
            FootPosition = footPosition;
            CoxaLength = coxaLength;
            FootOffset = Vector3.Zero;
        }

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

            Distance = baseToFoot.Length;
            var C = basePos.RotationComponent.Inverse() * baseToFoot.ToMatrix4();


            Angle1 = Trig.Atan2(-C.Y, C.X);

            var A = new Vector3(Angle1.Cos() * CoxaLength, - Angle1.Sin() * CoxaLength, 0);

            var AtoC = A - C.ToVector3();

            var lengthWithOffset = 0;

            //var relative = (basePos.RotationComponent * (baseToFoot.ToMatrix4()));


            //var y = Angle.FromRadians(tt.DotProduct(-Vector2.UnitX));



        }
    }
}
