using System;
using System.Collections.Generic;

namespace NQRW.Maths
{
    public class Vector2
    {
        /// <summary> X Component </summary>
        public double X { get; private set; }
        /// <summary> Y Component </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Create new Vect3 from doubles
        /// </summary>
        /// <param name="x">X Component</param>
        /// <param name="y">Y Component</param>
        /// <param name="z">Z Component</param>
        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Create new Vect3 from list of doubles
        /// </summary>
        /// <param name="a"></param>
        public Vector2(IList<double> a)
        {
            if (a.Count != 2) throw new ArgumentException("Array should be double[2]");
            X = a[0];
            Y = a[1];
        }

        /// <summary>
        /// Create new Vect3 from list of floats
        /// </summary>
        /// <param name="a"></param>
        public Vector2(IList<float> a)
        {
            if (a.Count != 2) throw new ArgumentException("Array should be double[2]");
            X = a[0];
            Y = a[1];
        }

        /// <summary>
        /// Calculate the length
        /// </summary>
        public double Length
        {
            get { return Math.Sqrt(LengthSquared); }
        }
        /// <summary>
        /// Calculate the square of the length
        /// </summary>
        public double LengthSquared
        {
            get { return X * X + Y * Y; }
        }

        /// <summary>
        /// A zero vector
        /// </summary>
        public static Vector2 Zero
        {
            get { return new Vector2(0.0, 0.0); }
        }

        /// <summary>
        /// A unit vector in the X direction
        /// </summary>
        public static Vector2 UnitX
        {
            get { return new Vector2(1.0, 0.0); }
        }

        /// <summary>
        /// A unit vector in the Y direction
        /// </summary>
        public static Vector2 UnitY
        {
            get { return new Vector2(0.0, 1.0); }
        }


        /// <summary>
        /// Returns a unit Vector
        /// </summary>
        /// <returns>Normalized Vector</returns>
        public Vector2 Normalize()
        {
            var num = 1f / Length;
            return new Vector2(X * num, Y * num);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        /// <summary>
        /// Subtract a Vector
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        /// <summary>
        /// Invert Vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        /// <summary>
        /// Multiply by double
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vector2 operator *(Vector2 v, double d)
        {
            return new Vector2(v.X * d, v.Y * d);
        }

        /// <summary>
        /// Multiply by double
        /// </summary>
        /// <param name="d"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 operator *(double d, Vector2 v)
        {
            return v * d;
        }

        /// <summary>
        /// Divide by double
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vector2 operator /(Vector2 v, double d)
        {
            return new Vector2(v.X / d, v.Y / d);
        }

        /// <summary>
        /// Test for equality
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a.Equals(b);
        }

        /// <summary>
        /// Test for not equality
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a == b);
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vector2)obj);
        }

        protected bool Equals(Vector2 other)
        {
            return X.NearlyEquals(other.X) && Y.NearlyEquals(other.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public double[] ToArray()
        {
            return new[] { X, Y };
        }

        public Vector2 Lerp(Vector2 end, double t)
        {
            return (1 - t) * this + t * end;
        }

        public override string ToString()
        {
            return String.Format("Vect3<{0},{1}>", X, Y);
        }
    }
}
