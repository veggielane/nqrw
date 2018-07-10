using System;

namespace NQRW.Maths
{
    public static class MathsHelper
    {
        public static Vector3 ToVector3(this Vector2 v, double z)
        {
            return new Vector3(v, z);
        }

        public static Vector2 ToVector2(this Vector3 v)
        {
            return new Vector2(v.X, v.Y);
        }
        public static Vector3 ToVector3(this Matrix4 m)
        {
            return new Vector3(m.X, m.Y, m.Z);
        }

        public static Vector2 Rotate(this Vector2 v, Angle a)
        {
            return new Vector2(v.X * a.Cos() - v.Y * a.Sin(), v.X * a.Sin() + v.Y * a.Cos());
        }
        
        public static Matrix4 ToMatrix4(this Vector3 v)
        {
            return Matrix4.Translate(v);
        }
        public static double Pow(this double x, double power)
        {
            return Math.Pow(x, power);
        }
        public static double Sqrt(this double x)
        {
            return Math.Pow(x, 0.5);
        }
        public static double Sin(this Angle a)
        {
            return Math.Sin(a.Radians);
        }
        public static double Cos(this Angle a)
        {
            return Math.Cos(a.Radians);
        }
        public static double Tan(this Angle a)
        {
            return Math.Tan(a.Radians);
        }
        public static double Map(this double x, double inMin, double inMax, double outMin, double outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }
        public static bool NearlyEquals(this double x, double y, double epsilon = 0.0000001)
        {
            return Math.Abs(x - y) <= Math.Abs(x * .00001);
        }
        public static bool NearlyEquals(this float x, float y, float epsilon = 0.0000001f)
        {
            return Math.Abs(x - y) <= Math.Abs(x * .00001);
        }

        public static bool NearlyLessThanOrEquals(this double x, double y, double epsilon = 0.0000001)
        {
            return x <= y || x.NearlyEquals(y, epsilon);
        }

        public static bool NearlyGreaterThanOrEquals(this double x, double y, double epsilon = 0.0000001)
        {
            return x >= y || x.NearlyEquals(y, epsilon);
        }

        public static bool NearlyLessThanOrEquals(this float x, float y, float epsilon = 0.0000001f)
        {
            return x <= y || x.NearlyEquals(y, epsilon);
        }

        public static bool NearlyGreaterThanOrEquals(this float x, float y, float epsilon = 0.0000001f)
        {
            return x >= y || x.NearlyEquals(y, epsilon);
        }




        public static double Max(double x, double y)
        {
            return Math.Max(x, y);
        }

        public static double Max(double x, double y, double z)
        {
            return Math.Max(x, Math.Max(y, z));
        }

        public static double Max(double w, double x, double y, double z)
        {
            return Math.Max(w, Math.Max(x, Math.Max(y, z)));
        }



        public static double Min(double x, double y)
        {
            return Math.Max(x, y);
        }
        public static double Min(double x, double y, double z)
        {
            return Math.Min(x, Math.Min(y, z));
        }
        public static double Min(double w, double x, double y, double z)
        {
            return Math.Min(w, Math.Min(x, Math.Min(y, z)));
        }
    }
}
