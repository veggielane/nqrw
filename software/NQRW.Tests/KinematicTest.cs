using NQRW.Kinematics;
using NQRW.Maths;
using NQRW.Robotics;
using NQRW.Settings;
using Xunit;
// ReSharper disable InconsistentNaming

namespace NQRW.Tests
{
    public class KinematicTest
    {
        [Fact]
        public void LegTestg()
        {
            var robot = new TestRobot(20, 76, 76, 96, 150);

            robot.Body.Z = robot.F;

            robot.Body.Roll = Angle.FromDegrees(5);
            robot.Body.Pitch = Angle.FromDegrees(5);
            robot.Body.Yaw = Angle.FromDegrees(5);

            robot.InverseKinematics();


            Assert.Equal(158.053206, robot.lf.Distance, 6);
            Assert.Equal(164.766584, robot.lm.Distance, 6);
            Assert.Equal(173.524093, robot.lr.Distance, 6);

            Assert.Equal(175.979834, robot.rf.Distance, 6);
            Assert.Equal(156.073840, robot.rm.Distance, 6);
            Assert.Equal(137.452465, robot.rr.Distance, 6);

            Assert.Equal(8.152048, robot.lf.Angle1.Degrees, 6);
            Assert.Equal(8.035559, robot.lm.Angle1.Degrees, 6);
            Assert.Equal(7.381673, robot.lr.Angle1.Degrees, 6);
            Assert.Equal(9.877819, robot.rf.Angle1.Degrees, 6);
            Assert.Equal(10.768978, robot.rm.Angle1.Degrees, 6);
            Assert.Equal(11.223463, robot.rr.Angle1.Degrees, 6);


            Assert.Equal(46.31991125865, robot.lf.Angle2.Degrees, 6);
            Assert.Equal(41.94122278606, robot.lm.Angle2.Degrees, 6);
            Assert.Equal(29.49809188575, robot.lr.Angle2.Degrees, 6);
            Assert.Equal(50.53421042468, robot.rr.Angle2.Degrees, 6);
            Assert.Equal(90.18196678411, robot.lf.Angle3.Degrees, 6);
            Assert.Equal(68.16605667929, robot.lm.Angle3.Degrees, 6);
            Assert.Equal(32.07557756263, robot.lr.Angle3.Degrees, 6);
            Assert.Equal(25.81105087355, robot.rr.Angle3.Degrees, 6);
            Assert.Equal(46.13794447453, robot.lf.Angle4.Degrees, 6);
            Assert.Equal(63.77516610678, robot.lm.Angle4.Degrees, 6);
            Assert.Equal(87.42251432312, robot.lr.Angle4.Degrees, 6);
            Assert.Equal(114.7231595511, robot.rr.Angle4.Degrees, 6);



        }

    }

    public class TestRobot : BaseRobot
    {

        public double A { get; set; } = 200.0;
        public double B { get; set; } = 200.0;
        public double C { get; set; } = 150.0;
        public double D { get; set; } = 160.0;
        public double E { get; set; } = 150.0;
        public double F { get; set; } = 50.0;

        public Leg4DOF lf { get; }
        public Leg4DOF lm { get; }
        public Leg4DOF lr { get; }
        public Leg4DOF rf { get; }
        public Leg4DOF rm { get; }
        public Leg4DOF rr { get; }

        public TestRobot(double coxaLength, double femurLength, double tibiaLength, double tarsusLength, double footPosition) : base("TestRobot")
        {

            var legSettings = new LegSettings(coxaLength, femurLength, tibiaLength, tarsusLength);

            lf = new Leg4DOF(Matrix4.Translate(-C, A, 0) * Matrix4.RotateZ(Angle.FromDegrees(180)), new Vector3(-C, A, 0) - new Vector3(footPosition, 0, 0), legSettings);
            lm = new Leg4DOF(Matrix4.Translate(-D, 0, 0) * Matrix4.RotateZ(Angle.FromDegrees(180)), new Vector3(-D, 0, 0) - new Vector3(footPosition, 0, 0), legSettings);
            lr = new Leg4DOF(Matrix4.Translate(-E, -B, 0) * Matrix4.RotateZ(Angle.FromDegrees(180)), new Vector3(-E, -B, 0) - new Vector3(footPosition, 0, 0), legSettings);

            rf = new Leg4DOF(Matrix4.Translate(C, A, 0), new Vector3(C, A, 0) + new Vector3(footPosition, 0, 0), legSettings);
            rm = new Leg4DOF(Matrix4.Translate(D, 0, 0), new Vector3(D, 0, 0) + new Vector3(footPosition, 0, 0), legSettings);
            rr = new Leg4DOF(Matrix4.Translate(E, -B, 0), new Vector3(E, -B, 0) + new Vector3(footPosition, 0, 0), legSettings);

            Legs.Add(Leg.LeftFront, lf);
            Legs.Add(Leg.LeftMiddle, lm);
            Legs.Add(Leg.LeftRear, lr);
            Legs.Add(Leg.RightFront, rf);
            Legs.Add(Leg.RightMiddle, rm);

            Legs.Add(Leg.RightRear, rr);
            Body = new Body();
        }

        public override void Boot()
        {

        }

        public override void Dispose()
        {

        }
    }
}
