using System;
using NQRW.Devices;
using NQRW.Maths;
using NQRW.Settings;

namespace NQRW.Kinematics
{
    public class Leg4DOF : ILeg
    {
        private readonly LegSettings _settings;
        public Matrix4 BasePosition { get; private set; }
        public Vector3 FootPosition { get; set; }
        public Vector3 FootOffset { get; set; }

        public double Distance { get; private set; }
        public Angle Angle1 { get; private set; } = Angle.Zero;
        public Angle Angle2 { get; private set; } = Angle.Zero;
        public Angle Angle3 { get; private set; } = Angle.Zero;
        public Angle Angle4 { get; private set; } = Angle.Zero;

        public readonly Servo CoxaServo;
        public readonly Servo FemurServo;
        public readonly Servo TibiaServo;
        public readonly Servo TarsusServo;

        public Leg4DOF(Matrix4 basePosition, Vector3 footPosition, LegSettings settings)
        {
            _settings = settings;
            BasePosition = basePosition;
            FootPosition = footPosition;
            FootOffset = Vector3.Zero;
            CoxaServo = new Servo(Angle.Zero, settings.CoxaOffset);
            FemurServo = new Servo(Angle.Zero, settings.FemurOffset);
            TibiaServo = new Servo(Angle.Zero, settings.TibiaOffset);
            TarsusServo = new Servo(Angle.Zero, settings.TarsusOffset);
        }

        public void Update(Matrix4 bodyPosition)
        {
            var basePos = bodyPosition * BasePosition;
            var baseToFoot = FootPosition + FootOffset - basePos.ToVector3();
            var foot = basePos.RotationComponent.Inverse() * baseToFoot.ToMatrix4();
            Distance = baseToFoot.Length;

            var zDash = Math.Abs(-foot.Z);
            var xDash = Math.Sqrt(Math.Pow(foot.X, 2) + Math.Pow(foot.Y, 2));

            var a = _settings.FemurLength;
            var b = _settings.TibiaLength;
            var c = _settings.TarsusLength;
            var d = Math.Sqrt(Math.Pow(xDash - _settings.CoxaLength, 2) + Math.Pow(zDash, 2));
            var thetaD = Angle.FromRadians(Math.Acos(zDash / d));
            var thetaB = Angle.FromRadians(Math.Acos((d * d + c * c - 2 * d * c * thetaD.Cos() - b * b - a * a) / (-2 * b * a)));
            var e = Math.Sqrt(d * d + c * c - 2 * d * c * thetaD.Cos());
            var thetaA1 = Angle.FromRadians(Math.Acos((a * a + e * e - b * b) / (2 * a * e)));
            var thetaA2 = Angle.FromRadians(Math.Acos((e * e + d * d - c * c) / (2 * e * d)));
            var thetaA = thetaA1 + thetaA2;
            var thetaC = Angle.TwoPI - thetaA - thetaB - thetaD;

            Angle1 = Trig.Atan2(-foot.Y, Math.Abs(foot.X));
            Angle2 = thetaD + thetaA - Angle.FromDegrees(90);
            Angle3 = Angle.PI - thetaB;
            Angle4 = Angle.PI - thetaC;

            CoxaServo.Angle = Angle.FromRadians((_settings.CoxaInvert ? -1 : 1) * Angle1);
            FemurServo.Angle = Angle.FromRadians((_settings.FemurInvert ? -1 : 1) * Angle2);
            TibiaServo.Angle = Angle.FromRadians((_settings.TibiaInvert ? -1 : 1) * Angle3);
            TarsusServo.Angle = Angle.FromRadians((_settings.TarsusInvert ? -1 : 1) * Angle4);
        }

        public override string ToString() => $"4DOF<{CoxaServo.Angle}, {FemurServo.Angle}, {TibiaServo.Angle}, {TarsusServo.Angle}>";
    }
}