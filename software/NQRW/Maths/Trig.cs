using System;

namespace NQRW.Maths
{
    public class Trig
    {
        public const double PI = Math.PI;
        public const double TwoPi = Math.PI * 2.0;
        public const double PiOverTwo = Math.PI / 2.0;

        public static Double Sqrt(Double d)
        {
            return Math.Pow(d, 0.5);
        }
        public static Double Pow(Double x, Double y)
        {
            return Math.Pow(x, y);
        }

        public static Double Sin(Angle angle)
        {
            return Math.Sin(angle.Radians);
        }
        public static Double Cos(Angle angle)
        {
            return Math.Cos(angle.Radians);
        }
        public static Double Tan(Angle angle)
        {
            return Math.Tan(angle.Radians);
        }

        public static Angle Acos(Double d)
        {
            return Angle.FromRadians(Math.Acos(d));
        }
        public static Angle Asin(Double d)
        {
            return Angle.FromRadians(Math.Asin(d));
        }
        public static Angle Atan(Double d)
        {
            return Angle.FromRadians(Math.Asin(d));
        }

        public static Angle Atan2(Double y, Double x)
        {
            return Angle.FromRadians(Math.Atan2(y, x));
        }

        public static Double Abs(Double d)
        {
            if (d >= 0.0)
                return d;
            return -d;
        }
    }
}
