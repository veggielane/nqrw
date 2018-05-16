using NQRW.Kinematics;
using NQRW.Maths;
using NQRW.Robotics;
using System;
using Xunit;

namespace NQRW.Tests
{
    public class KinematicTest
    {
        [Fact]
        public void LegTest()
        {
            var A = 200.0;
            var B = 200.0;

            var C = 150.0;
            var D = 160.0;
            var E = 150.0;
            var F = 50.0;

            var kine = new KinematicEngine();
            var footPosition = 150.0;

            kine.BodyZ = F;

            kine.BodyRoll = Angle.FromDegrees(10);
            kine.BodyPitch = Angle.FromDegrees(10);
            kine.BodyYaw = Angle.FromDegrees(10);

            var coxa = 20.0;
            var lf = new Leg4DOF(Matrix4.Translate(-C, A, 0) * Matrix4.RotateZ(Angle.FromDegrees(180)), new Vector3(-C, A, 0) - new Vector3(footPosition, 0, 0), coxa);
            var lm = new Leg4DOF(Matrix4.Translate(-D, 0, 0) * Matrix4.RotateZ(Angle.FromDegrees(180)), new Vector3(-D, 0, 0) - new Vector3(footPosition, 0, 0), coxa);
            var lr = new Leg4DOF(Matrix4.Translate(-E, -B, 0) * Matrix4.RotateZ(Angle.FromDegrees(180)), new Vector3(-E, -B, 0) - new Vector3(footPosition, 0, 0), coxa);

            var rf = new Leg4DOF(Matrix4.Translate(C, A, 0), new Vector3(C, A, 0) + new Vector3(footPosition, 0, 0), coxa);
            var rm = new Leg4DOF(Matrix4.Translate(D, 0, 0), new Vector3(D, 0, 0) + new Vector3(footPosition, 0, 0), coxa);
            var rr = new Leg4DOF(Matrix4.Translate(E, -B, 0), new Vector3(E, -B, 0) + new Vector3(footPosition, 0, 0), coxa);

            kine.Legs.Add(Leg.LeftFront, lf);
            kine.Legs.Add(Leg.LeftMiddle, lm);
            kine.Legs.Add(Leg.LeftRear, lr);
            kine.Legs.Add(Leg.RightFront, rf);
            kine.Legs.Add(Leg.RightMiddle, rm);
            kine.Legs.Add(Leg.RightRear, rr);

            kine.Update();

            Assert.Equal(170.3966749925, lf.Distance, 6);
            Assert.Equal(175.4127043623, lm.Distance, 6);
            Assert.Equal(188.6501832172, lr.Distance, 6);

            Assert.Equal(192.9244649192, rf.Distance, 6);
            Assert.Equal(158.7856264476, rm.Distance, 6);
            Assert.Equal(130.3271984353, rr.Distance, 6);

            Assert.Equal(16.39834173887, lf.Angle1.Degrees, 6);
            Assert.Equal(14.95776090508, lm.Angle1.Degrees, 6);
            Assert.Equal(13.12864851006, lr.Angle1.Degrees, 6);

            Assert.Equal(16.92111435065, rf.Angle1.Degrees, 6);
            Assert.Equal(19.32108487452, rm.Angle1.Degrees, 6);
            Assert.Equal(21.71047817322, rr.Angle1.Degrees, 6);

            
                
                
        }
        [Fact]
        public void LegTest2()
        {
            var A = 200.0;
            var B = 200.0;

            var C = 150.0;
            var D = 160.0;
            var E = 150.0;
            var F = 50.0;

            var kine = new KinematicEngine();
            var footPosition = 150.0;

            kine.BodyZ = F;

            kine.BodyRoll = Angle.FromDegrees(5);
            kine.BodyPitch = Angle.FromDegrees(5);
            kine.BodyYaw = Angle.FromDegrees(5);
            var coxa = 20.0;

            var lf = new Leg4DOF(Matrix4.Translate(-C, A, 0) * Matrix4.RotateZ(Angle.FromDegrees(180)), new Vector3(-C, A, 0) - new Vector3(footPosition, 0, 0), coxa);
            var lm = new Leg4DOF(Matrix4.Translate(-D, 0, 0) * Matrix4.RotateZ(Angle.FromDegrees(180)), new Vector3(-D, 0, 0) - new Vector3(footPosition, 0, 0), coxa);
            var lr = new Leg4DOF(Matrix4.Translate(-E, -B, 0) * Matrix4.RotateZ(Angle.FromDegrees(180)), new Vector3(-E, -B, 0) - new Vector3(footPosition, 0, 0), coxa);

            var rf = new Leg4DOF(Matrix4.Translate(C, A, 0), new Vector3(C, A, 0) + new Vector3(footPosition, 0, 0), coxa);
            var rm = new Leg4DOF(Matrix4.Translate(D, 0, 0), new Vector3(D, 0, 0) + new Vector3(footPosition, 0, 0), coxa);
            var rr = new Leg4DOF(Matrix4.Translate(E, -B, 0), new Vector3(E, -B, 0) + new Vector3(footPosition, 0, 0), coxa);

            kine.Legs.Add(Leg.LeftFront, lf);
            kine.Legs.Add(Leg.LeftMiddle, lm);
            kine.Legs.Add(Leg.LeftRear, lr);
            kine.Legs.Add(Leg.RightFront, rf);
            kine.Legs.Add(Leg.RightMiddle, rm);
            kine.Legs.Add(Leg.RightRear, rr);

            kine.Update();

            Assert.Equal(158.053206, lf.Distance, 6);
            Assert.Equal(164.766584, lm.Distance, 6);
            Assert.Equal(173.524093, lr.Distance, 6);

            Assert.Equal(175.979834, rf.Distance, 6);
            Assert.Equal(156.073840, rm.Distance, 6);
            Assert.Equal(137.452465, rr.Distance, 6);

            Assert.Equal(8.152048, lf.Angle1.Degrees, 6);
            Assert.Equal(8.035559, lm.Angle1.Degrees, 6);
            Assert.Equal(7.381673, lr.Angle1.Degrees, 6);

            Assert.Equal(9.877819, rf.Angle1.Degrees, 6);
            Assert.Equal(10.768978, rm.Angle1.Degrees, 6);
            Assert.Equal(11.223463, rr.Angle1.Degrees, 6);

        }

    }
}
