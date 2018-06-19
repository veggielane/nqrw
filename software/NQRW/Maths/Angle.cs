using System;

namespace NQRW.Maths
{
    public class Angle
    {
        public static Angle Zero => new Angle(0);
        public static readonly Angle PI = FromRadians(Math.PI);
        public static readonly Angle TwoPI = FromRadians(Math.PI * 2);

        public readonly double Radians;
        public double Degrees => Radians * 180.0 / Math.PI;
        public double Gradians => Radians * 50 / Math.PI;

        private Angle(double radians)
        {
            Radians = radians;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Angle))
                return false;
            return Radians.NearlyEquals(((Angle)obj).Radians);
        }
        public bool Equals(Angle other)
        {
            return Radians.Equals(other.Radians);
        }
        public override int GetHashCode()
        {
            return Radians.GetHashCode();
        }

        public static Angle FromRadians(double radians) => new Angle(radians);
        public static Angle FromDegrees(double degrees) => new Angle(degrees * Math.PI / 180.0);
        public static Angle FromGradians(double gradians) => new Angle(gradians * Math.PI / 50);
        public static Angle operator +(Angle a1, Angle a2) => new Angle(a1.Radians + a2.Radians);
        public static Angle operator -(Angle a1, Angle a2) => new Angle(a1.Radians - a2.Radians);
        public static Angle operator *(Angle a, double d) => new Angle(a.Radians * d);
        public static Angle operator /(Angle a, double d) => new Angle(a.Radians / d);
        public static implicit operator double(Angle angle) => angle.Radians;
        public static implicit operator Angle(double radians) => FromRadians(radians);
        public override string ToString() => "Angle<" + Degrees + "\x00B0>";
    }
}
