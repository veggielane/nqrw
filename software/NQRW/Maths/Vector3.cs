using System;
using System.Collections.Generic;

namespace NQRW.Maths
{
    public class Vector3
    {
        /// <summary> X Component </summary>
        public double X { get; private set; }
        /// <summary> Y Component </summary>
        public double Y { get; private set; }
        /// <summary> Z Component </summary>
        public double Z { get; private set; }

        /// <summary>
        /// Create new Vect3 from doubles
        /// </summary>
        /// <param name="x">X Component</param>
        /// <param name="y">Y Component</param>
        /// <param name="z">Z Component</param>
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector2 v, double z)
        {
            X = v.X;
            Y = v.Y;
            Z = z;
        }

        /// <summary>
        /// Create new Vect3 from list of doubles
        /// </summary>
        /// <param name="a"></param>
        public Vector3(IList<double> a)
        {
            if (a.Count != 3) throw new ArgumentException("Array should be double[3]");
            X = a[0];
            Y = a[1];
            Z = a[2];
        }

        /// <summary>
        /// Create new Vect3 from list of floats
        /// </summary>
        /// <param name="a"></param>
        public Vector3(IList<float> a)
        {
            if (a.Count != 3) throw new ArgumentException("Array should be double[3]");
            X = a[0];
            Y = a[1];
            Z = a[2];
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
            get { return X * X + Y * Y + Z * Z; }
        }

        /// <summary>
        /// A zero vector
        /// </summary>
        public static Vector3 Zero
        {
            get { return new Vector3(0.0, 0.0, 0.0); }
        }

        /// <summary>
        /// A unit vector in the X direction
        /// </summary>
        public static Vector3 UnitX
        {
            get { return new Vector3(1.0, 0.0, 0.0); }
        }

        /// <summary>
        /// A unit vector in the Y direction
        /// </summary>
        public static Vector3 UnitY
        {
            get { return new Vector3(0.0, 1.0, 0.0); }
        }

        /// <summary>
        /// A unit vector in the Z direction
        /// </summary>
        public static Vector3 UnitZ
        {
            get { return new Vector3(0.0, 0.0, 1.0); }
        }

        /// <summary>
        /// Returns a unit Vector
        /// </summary>
        /// <returns>Normalized Vector</returns>
        public Vector3 Normalize()
        {
            var num = 1f / Length;
            return new Vector3(X * num, Y * num, Z * num);
        }

        /// <summary>
        /// Calculate the Cross Product
        /// </summary>
        /// <param name="b">Other Vector</param>
        /// <returns>Cross Product</returns>
        public Vector3 CrossProduct(Vector3 b)
        {
            return new Vector3(Y * b.Z - Z * b.Y, Z * b.X - X * b.Z, X * b.Y - Y * b.X);
        }

        /// <summary>
        /// Calculate the Dot Product
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double DotProduct(Vector3 v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        /// <summary>
        /// Add a Vector
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Subtract a Vector
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Invert Vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        /// <summary>
        /// Multiply by double
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 v, double d)
        {
            return new Vector3(v.X * d, v.Y * d, v.Z * d);
        }

        /// <summary>
        /// Multiply by double
        /// </summary>
        /// <param name="d"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 operator *(double d, Vector3 v)
        {
            return v * d;
        }

        /// <summary>
        /// Divide by double
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vector3 operator /(Vector3 v, double d)
        {
            return new Vector3(v.X / d, v.Y / d, v.Z / d);
        }

        /// <summary>
        /// Test for equality
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Vector3 a, Vector3 b)
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
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vector3)obj);
        }

        protected bool Equals(Vector3 other)
        {
            return X.NearlyEquals(other.X) && Y.NearlyEquals(other.Y) && Z.NearlyEquals(other.Z);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public double[] ToArray()
        {
            return new[] { X, Y, Z };
        }

        public Vector3 Lerp(Vector3 end, double t)
        {
            return (1 - t) * this + t * end;
        }

        public override string ToString()
        {
            return String.Format("Vect3<{0},{1},{2}>", X, Y, Z);
        }
    }
}
