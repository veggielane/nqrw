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

        public Leg4DOF(Matrix4 basePosition, Vector3 footPosition)
        {
            BasePosition = basePosition;
            FootPosition = footPosition;
            FootOffset = Vector3.Zero;
        }

        public double CoxaLength { get; set; }
        public double FemurLength { get; set; }
        public double TibiaLength { get; set; }
        public double TarsusLength { get; set; }


        public void Update(Matrix4 bodyPosition)
        {
            var basePos = bodyPosition * BasePosition;
            var baseToFoot = (FootPosition + FootOffset) - basePos.ToVector3();

            var distance = baseToFoot.Length;
            var relative = (basePos.RotationComponent.Inverse()) * baseToFoot.ToMatrix4();
            var mainAngle = Angle.FromDegrees(180.0) - Trig.Atan2(relative.Y, relative.X);
            var y = relative.ToVector3();

            //var relative = (basePos.RotationComponent * (baseToFoot.ToMatrix4()));


            //var y = Angle.FromRadians(tt.DotProduct(-Vector2.UnitX));



        }
    }
}
