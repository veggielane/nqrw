using System;
using System.Text;

namespace NQRW.Maths
{
    public class Angle
    {
        public readonly double Radians;

        public double Degrees
        {
            get { return Radians * 180.0 / Math.PI; }
        }

        public double Gradians
        {
            get { return Radians * 50 / Math.PI; }
        }

        private Angle(double radians)
        {
            Radians = radians;
        }

        public static Angle FromRadians(double radians)
        {
            return new Angle(radians);
        }

        public static Angle FromDegrees(double degrees)
        {
            return new Angle(degrees * Math.PI / 180.0);
        }

        public static Angle FromGradians(double gradians)
        {
            return new Angle(gradians * Math.PI / 50);
        }

        public override string ToString()
        {
            return "Angle<" + Degrees + "\x00B0>";
        }

        public static Angle Zero
        {
            get { return new Angle(0); }
        }

        public static Angle operator +(Angle a1, Angle a2)
        {
            return new Angle(a1.Radians + a2.Radians);
        }

        public static Angle operator -(Angle a1, Angle a2)
        {
            return new Angle(a1.Radians - a2.Radians);
        }

        public static Angle operator *(Angle a, double d)
        {
            return new Angle(a.Radians * d);
        }

        public static Angle operator /(Angle a, double d)
        {
            return new Angle(a.Radians / d);
        }

        public static implicit operator double(Angle angle)
        {
            return angle.Radians;
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

        public static readonly Angle PI = FromRadians(Math.PI);
        public static readonly Angle TwoPI = FromRadians(Math.PI*2);
    }
}
