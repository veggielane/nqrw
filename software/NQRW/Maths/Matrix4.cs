﻿using System;

namespace NQRW.Maths
{
    public class Matrix4
    {
        private readonly Double[,] _data;

        public Matrix4(Double[,] data)
        {
            _data = data;
        }

        public Double this[int i, int j]
        {
            get
            {
                if (i < 1 || i > 4 || j < 1 || j > 4)
                    throw new ArgumentOutOfRangeException();
                return _data[i - 1, j - 1];
            }
            set
            {
                if (i < 1 || i > 4 || j < 1 || j > 4)
                    throw new ArgumentOutOfRangeException();
                _data[i - 1, j - 1] = value;
            }
        }

        public Double X
        {
            get { return this[1, 4]; }
            set { this[1, 4] = value; }
        }

        public Double Y
        {
            get { return this[2, 4]; }
            set { this[2, 4] = value; }
        }

        public Double Z
        {
            get { return this[3, 4]; }
            set { this[3, 4] = value; }
        }

        public Double LengthSquared
        {
            get { return Math.Pow(this[1, 4], 2.0) + Math.Pow(this[2, 4], 2.0) + Math.Pow(this[3, 4], 2.0); }
        }

        public Double Length
        {
            get { return Math.Sqrt(LengthSquared); }
        }

        public static Matrix4 Identity
        {
            get
            {
                return new Matrix4(new[,]
                    {
                        {1.0, 0.0, 0.0, 0.0},
                        {0.0, 1.0, 0.0, 0.0},
                        {0.0, 0.0, 1.0, 0.0},
                        {0.0, 0.0, 0.0, 1.0}
                    });
            }
        }

        public Matrix4 Inverse()
        {
            return Adjugate() / Determinant();
        }

        public Matrix4 Cofactor()
        {
            return new Matrix4(new[,]{
                {
                    -(this[2, 4] * this[3, 3] * this[4, 2]) + this[2, 3] * this[3, 4] * this[4, 2] + this[2, 4] * this[3, 2] * this[4, 3] - this[2, 2] * this[3, 4] * this[4, 3] - this[2, 3] * this[3, 2] * this[4, 4] + this[2, 2] * this[3, 3] * this[4, 4], this[2, 4] * this[3, 3] * this[4, 1] - this[2, 3] * this[3, 4] * this[4, 1] - this[2, 4] * this[3, 1] * this[4, 3] + this[2, 1] * this[3, 4] * this[4, 3] + this[2, 3] * this[3, 1] * this[4, 4] - this[2, 1] * this[3, 3] * this[4, 4], -(this[2, 4] * this[3, 2] * this[4, 1]) + this[2, 2] * this[3, 4] * this[4, 1] + this[2, 4] * this[3, 1] * this[4, 2] - this[2, 1] * this[3, 4] * this[4, 2] - this[2, 2] * this[3, 1] * this[4, 4] + this[2, 1] * this[3, 2] * this[4, 4], this[2, 3] * this[3, 2] * this[4, 1] - this[2, 2] * this[3, 3] * this[4, 1] - this[2, 3] * this[3, 1] * this[4, 2] + this[2, 1] * this[3, 3] * this[4, 2] + this[2, 2] * this[3, 1] * this[4, 3] - this[2, 1] * this[3, 2] * this[4, 3]
                },
                {
                    this[1, 4] * this[3, 3] * this[4, 2] - this[1, 3] * this[3, 4] * this[4, 2] - this[1, 4] * this[3, 2] * this[4, 3] + this[1, 2] * this[3, 4] * this[4, 3] + this[1, 3] * this[3, 2] * this[4, 4] - this[1, 2] * this[3, 3] * this[4, 4], -(this[1, 4] * this[3, 3] * this[4, 1]) + this[1, 3] * this[3, 4] * this[4, 1] + this[1, 4] * this[3, 1] * this[4, 3] - this[1, 1] * this[3, 4] * this[4, 3] - this[1, 3] * this[3, 1] * this[4, 4] + this[1, 1] * this[3, 3] * this[4, 4], this[1, 4] * this[3, 2] * this[4, 1] - this[1, 2] * this[3, 4] * this[4, 1] - this[1, 4] * this[3, 1] * this[4, 2] + this[1, 1] * this[3, 4] * this[4, 2] + this[1, 2] * this[3, 1] * this[4, 4] - this[1, 1] * this[3, 2] * this[4, 4], -(this[1, 3] * this[3, 2] * this[4, 1]) + this[1, 2] * this[3, 3] * this[4, 1] + this[1, 3] * this[3, 1] * this[4, 2] - this[1, 1] * this[3, 3] * this[4, 2] - this[1, 2] * this[3, 1] * this[4, 3] + this[1, 1] * this[3, 2] * this[4, 3]
                },
                {
                    -(this[1, 4] * this[2, 3] * this[4, 2]) + this[1, 3] * this[2, 4] * this[4, 2] + this[1, 4] * this[2, 2] * this[4, 3] - this[1, 2] * this[2, 4] * this[4, 3] - this[1, 3] * this[2, 2] * this[4, 4] + this[1, 2] * this[2, 3] * this[4, 4], this[1, 4] * this[2, 3] * this[4, 1] - this[1, 3] * this[2, 4] * this[4, 1] - this[1, 4] * this[2, 1] * this[4, 3] + this[1, 1] * this[2, 4] * this[4, 3] + this[1, 3] * this[2, 1] * this[4, 4] - this[1, 1] * this[2, 3] * this[4, 4], -(this[1, 4] * this[2, 2] * this[4, 1]) + this[1, 2] * this[2, 4] * this[4, 1] + this[1, 4] * this[2, 1] * this[4, 2] - this[1, 1] * this[2, 4] * this[4, 2] - this[1, 2] * this[2, 1] * this[4, 4] + this[1, 1] * this[2, 2] * this[4, 4], this[1, 3] * this[2, 2] * this[4, 1] - this[1, 2] * this[2, 3] * this[4, 1] - this[1, 3] * this[2, 1] * this[4, 2] + this[1, 1] * this[2, 3] * this[4, 2] + this[1, 2] * this[2, 1] * this[4, 3] - this[1, 1] * this[2, 2] * this[4, 3]
                },
                {
                    this[1, 4] * this[2, 3] * this[3, 2] - this[1, 3] * this[2, 4] * this[3, 2] - this[1, 4] * this[2, 2] * this[3, 3] + this[1, 2] * this[2, 4] * this[3, 3] + this[1, 3] * this[2, 2] * this[3, 4] - this[1, 2] * this[2, 3] * this[3, 4], -(this[1, 4] * this[2, 3] * this[3, 1]) + this[1, 3] * this[2, 4] * this[3, 1] + this[1, 4] * this[2, 1] * this[3, 3] - this[1, 1] * this[2, 4] * this[3, 3] - this[1, 3] * this[2, 1] * this[3, 4] + this[1, 1] * this[2, 3] * this[3, 4], this[1, 4] * this[2, 2] * this[3, 1] - this[1, 2] * this[2, 4] * this[3, 1] - this[1, 4] * this[2, 1] * this[3, 2] + this[1, 1] * this[2, 4] * this[3, 2] + this[1, 2] * this[2, 1] * this[3, 4] - this[1, 1] * this[2, 2] * this[3, 4], -(this[1, 3] * this[2, 2] * this[3, 1]) + this[1, 2] * this[2, 3] * this[3, 1] + this[1, 3] * this[2, 1] * this[3, 2] - this[1, 1] * this[2, 3] * this[3, 2] - this[1, 2] * this[2, 1] * this[3, 3] + this[1, 1] * this[2, 2] * this[3, 3]
                }});
        }

        public Matrix4 Adjugate()
        {
            return Cofactor().Transpose();
        }

        public Double Determinant()
        {
            return this[1, 1] * this[2, 2] * this[3, 3] * this[4, 4] + this[1, 1] * this[2, 3] * this[3, 4] * this[4, 2] + this[1, 1] * this[2, 4] * this[3, 2] * this[4, 3]
                   + this[1, 2] * this[2, 1] * this[3, 4] * this[4, 3] + this[1, 2] * this[2, 3] * this[3, 1] * this[4, 4] + this[1, 2] * this[2, 4] * this[3, 3] * this[4, 1]
                   + this[1, 3] * this[2, 1] * this[3, 2] * this[4, 4] + this[1, 3] * this[2, 2] * this[3, 4] * this[4, 1] + this[1, 3] * this[2, 4] * this[3, 1] * this[4, 1]
                   + this[1, 4] * this[2, 1] * this[3, 3] * this[4, 2] + this[1, 4] * this[2, 2] * this[3, 1] * this[4, 3] + this[1, 4] * this[2, 3] * this[3, 2] * this[4, 1]
                   - this[1, 1] * this[2, 2] * this[3, 4] * this[4, 3] - this[1, 1] * this[2, 3] * this[3, 2] * this[4, 4] - this[1, 1] * this[2, 4] * this[3, 3] * this[4, 2]
                   - this[1, 2] * this[2, 1] * this[3, 3] * this[4, 4] - this[1, 2] * this[2, 3] * this[3, 4] * this[4, 1] - this[1, 2] * this[2, 4] * this[3, 1] * this[4, 3]
                   - this[1, 3] * this[2, 1] * this[3, 4] * this[4, 2] - this[1, 3] * this[2, 2] * this[3, 1] * this[4, 4] - this[1, 3] * this[2, 4] * this[3, 2] * this[4, 1]
                   - this[1, 4] * this[2, 1] * this[3, 2] * this[4, 3] - this[1, 4] * this[2, 2] * this[3, 3] * this[4, 1] - this[1, 4] * this[2, 3] * this[3, 1] * this[4, 2];
        }

        public Matrix4 Add(Matrix4 matrix)
        {
            return new Matrix4(new[,]
                {
                    {
                        this[1, 1] + matrix[1, 1], this[1, 2] + matrix[1, 2], this[1, 3] + matrix[1, 3],
                        this[1, 4] + matrix[1, 4]
                    },
                    {
                        this[2, 1] + matrix[2, 1], this[2, 2] + matrix[2, 2], this[2, 3] + matrix[2, 3],
                        this[2, 4] + matrix[2, 4]
                    },
                    {
                        this[3, 1] + matrix[3, 1], this[3, 2] + matrix[3, 2], this[3, 3] + matrix[3, 3],
                        this[3, 4] + matrix[3, 4]
                    },
                    {
                        this[4, 1] + matrix[4, 1], this[4, 2] + matrix[4, 2], this[4, 3] + matrix[4, 3],
                        this[4, 4] + matrix[4, 4]
                    }
                });
        }

        public Matrix4 Add(Double scalar)
        {
            return new Matrix4(new[,]
                {
                    {this[1, 1] + scalar, this[1, 2] + scalar, this[1, 3] + scalar, this[1, 4] + scalar},
                    {this[2, 1] + scalar, this[2, 2] + scalar, this[2, 3] + scalar, this[2, 4] + scalar},
                    {this[3, 1] + scalar, this[3, 2] + scalar, this[3, 3] + scalar, this[3, 4] + scalar},
                    {this[4, 1] + scalar, this[4, 2] + scalar, this[4, 3] + scalar, this[4, 4] + scalar}
                });
        }

        public Matrix4 Subtract(Matrix4 matrix)
        {
            return new Matrix4(new[,]
                {
                    {
                        this[1, 1] - matrix[1, 1], this[1, 2] - matrix[1, 2], this[1, 3] - matrix[1, 3],
                        this[1, 4] - matrix[1, 4]
                    },
                    {
                        this[2, 1] - matrix[2, 1], this[2, 2] - matrix[2, 2], this[2, 3] - matrix[2, 3],
                        this[2, 4] - matrix[2, 4]
                    },
                    {
                        this[3, 1] - matrix[3, 1], this[3, 2] - matrix[3, 2], this[3, 3] - matrix[3, 3],
                        this[3, 4] - matrix[3, 4]
                    },
                    {
                        this[4, 1] - matrix[4, 1], this[4, 2] - matrix[4, 2], this[4, 3] - matrix[4, 3],
                        this[4, 4] - matrix[4, 4]
                    }
                });
        }

        public Matrix4 Subtract(Double scalar)
        {
            return new Matrix4(new[,]
                {
                    {this[1, 1] - scalar, this[1, 2] - scalar, this[1, 3] - scalar, this[1, 4] - scalar},
                    {this[2, 1] - scalar, this[2, 2] - scalar, this[2, 3] - scalar, this[2, 4] - scalar},
                    {this[3, 1] - scalar, this[3, 2] - scalar, this[3, 3] - scalar, this[3, 4] - scalar},
                    {this[4, 1] - scalar, this[4, 2] - scalar, this[4, 3] - scalar, this[4, 4] - scalar}
                });
        }

        public Matrix4 Multiply(Matrix4 matrix)
        {
            return new Matrix4(new[,]
                {
                    {
                        this[1, 1]*matrix[1, 1] + this[1, 2]*matrix[2, 1] + this[1, 3]*matrix[3, 1] +
                        this[1, 4]*matrix[4, 1],
                        this[1, 1]*matrix[1, 2] + this[1, 2]*matrix[2, 2] + this[1, 3]*matrix[3, 2] +
                        this[1, 4]*matrix[4, 2],
                        this[1, 1]*matrix[1, 3] + this[1, 2]*matrix[2, 3] + this[1, 3]*matrix[3, 3] +
                        this[1, 4]*matrix[4, 3],
                        this[1, 1]*matrix[1, 4] + this[1, 2]*matrix[2, 4] + this[1, 3]*matrix[3, 4] +
                        this[1, 4]*matrix[4, 4]
                    },
                    {
                        this[2, 1]*matrix[1, 1] + this[2, 2]*matrix[2, 1] + this[2, 3]*matrix[3, 1] +
                        this[2, 4]*matrix[4, 1],
                        this[2, 1]*matrix[1, 2] + this[2, 2]*matrix[2, 2] + this[2, 3]*matrix[3, 2] +
                        this[2, 4]*matrix[4, 2],
                        this[2, 1]*matrix[1, 3] + this[2, 2]*matrix[2, 3] + this[2, 3]*matrix[3, 3] +
                        this[2, 4]*matrix[4, 3],
                        this[2, 1]*matrix[1, 4] + this[2, 2]*matrix[2, 4] + this[2, 3]*matrix[3, 4] +
                        this[2, 4]*matrix[4, 4]
                    },
                    {
                        this[3, 1]*matrix[1, 1] + this[3, 2]*matrix[2, 1] + this[3, 3]*matrix[3, 1] +
                        this[3, 4]*matrix[4, 1],
                        this[3, 1]*matrix[1, 2] + this[3, 2]*matrix[2, 2] + this[3, 3]*matrix[3, 2] +
                        this[3, 4]*matrix[4, 2],
                        this[3, 1]*matrix[1, 3] + this[3, 2]*matrix[2, 3] + this[3, 3]*matrix[3, 3] +
                        this[3, 4]*matrix[4, 3],
                        this[3, 1]*matrix[1, 4] + this[3, 2]*matrix[2, 4] + this[3, 3]*matrix[3, 4] +
                        this[3, 4]*matrix[4, 4]
                    },
                    {
                        this[4, 1]*matrix[1, 1] + this[4, 2]*matrix[2, 1] + this[4, 3]*matrix[3, 1] +
                        this[4, 4]*matrix[4, 1],
                        this[4, 1]*matrix[1, 2] + this[4, 2]*matrix[2, 2] + this[4, 3]*matrix[3, 2] +
                        this[4, 4]*matrix[4, 2],
                        this[4, 1]*matrix[1, 3] + this[4, 2]*matrix[2, 3] + this[4, 3]*matrix[3, 3] +
                        this[4, 4]*matrix[4, 3],
                        this[4, 1]*matrix[1, 4] + this[4, 2]*matrix[2, 4] + this[4, 3]*matrix[3, 4] +
                        this[4, 4]*matrix[4, 4]
                    }
                });
        }

        public Matrix4 Multiply(Double scalar)
        {
            return new Matrix4(new[,]
                {
                    {this[1, 1]*scalar, this[1, 2]*scalar, this[1, 3]*scalar, this[1, 4]*scalar},
                    {this[2, 1]*scalar, this[2, 2]*scalar, this[2, 3]*scalar, this[2, 4]*scalar},
                    {this[3, 1]*scalar, this[3, 2]*scalar, this[3, 3]*scalar, this[3, 4]*scalar},
                    {this[4, 1]*scalar, this[4, 2]*scalar, this[4, 3]*scalar, this[4, 4]*scalar}
                });
        }


        public Matrix4 Divide(Double scalar)
        {
            return Multiply(1.0 / scalar);
        }

        public Matrix4 Transpose()
        {
            return new Matrix4(new[,]
                {
                    {this[1, 1], this[2, 1], this[3, 1], this[4, 1]},
                    {this[1, 2], this[2, 2], this[3, 2], this[4, 2]},
                    {this[1, 3], this[2, 3], this[3, 3], this[4, 3]},
                    {this[1, 4], this[2, 4], this[3, 4], this[4, 4]}
                });
        }

        public static Matrix4 Translate(Double x, Double y, Double z)
        {
            return new Matrix4(new[,]
                {
                    {1.0, 0.0, 0.0, x},
                    {0.0, 1.0, 0.0, y},
                    {0.0, 0.0, 1.0, z},
                    {0.0, 0.0, 0.0, 1.0}
                });
        }

        public static Matrix4 Translate(Vector3 v)
        {
            return Translate(v.X, v.Y, v.Z);
        }

        public static Matrix4 RotateX(Angle theta)
        {
            return new Matrix4(new[,]
                {
                    {1.0, 0.0, 0.0, 0.0},
                    {0.0, Trig.Cos(theta), Trig.Sin(theta), 0.0},
                    {0.0, -Trig.Sin(theta), Trig.Cos(theta), 0.0},
                    {0.0, 0.0, 0.0, 1.0}
                });
        }

        public static Matrix4 RotateY(Angle theta)
        {
            return new Matrix4(new[,]
                {
                    {Trig.Cos(theta), 0.0, -Trig.Sin(theta), 0.0},
                    {0.0, 1.0, 0.0, 0.0},
                    {Trig.Sin(theta), 0.0, Trig.Cos(theta), 0.0},
                    {0.0, 0.0, 0.0, 1.0}
                });
        }

        public static Matrix4 RotateZ(Angle theta)
        {
            return new Matrix4(new[,]
                {
                    {Trig.Cos(theta), Trig.Sin(theta), 0.0, 0.0},
                    {-Trig.Sin(theta), Trig.Cos(theta), 0.0, 0.0},
                    {0.0, 0.0, 1.0, 0.0},
                    {0.0, 0.0, 0.0, 1.0}
                });
        }

        public static Matrix4 Scale(double s)
        {
            return new Matrix4(new[,]
            {
                {s, 0, 0, 0},
                {0, s, 0, 0},
                {0, 0, s, 0},
                {0, 0, 0, 1}
            });
        }


        /*
        public static Mat4 Rotate(Vect3 axis, Angle theta)
        {
            return new Mat4(new[,]
                {
                    {theta.Cos() + axis.X * axis.X * (1 - theta.Cos()), axis.X*axis.Y*(1 - theta.Cos()) - axis.Z * theta.Sin(), axis.X * axis.Z * (1 - theta.Cos()) + axis.Y*theta.Sin(), 0.0},
                    {axis.Y*axis.X*(1-theta.Cos())+axis.Z*theta.Sin(), theta.Cos() + axis.Y * axis.Y * (1 - theta.Cos()), axis.Y * axis.Z * (1 - theta.Cos()) - axis.X*theta.Sin(), 0.0},
                    {axis.Z*axis.X*(1-theta.Cos())-axis.Y*theta.Sin(), axis.Z*axis.Y*(1 - theta.Cos()) + axis.X * theta.Sin(), theta.Cos() + axis.Z * axis.Z * (1 - theta.Cos()), 0.0},
                    {0.0, 0.0, 0.0, 1.0}
                });
        }
        public static Mat4 RotateNormal(Vect3 normal)
        {
            var axis = Vect3.UnitZ.CrossProduct(normal);
            var angle = Vect3.UnitZ.DotProduct(normal)/(normal.Length);
            return Rotate(axis, Angle.FromRadians(angle));
        }*/



        public override String ToString()
        {
            return "Matrix <4x4>:\n"
                   + "|" + this[1, 1] + "," + this[1, 2] + "," + this[1, 3] + "," + this[1, 4] + "|\n"
                   + "|" + this[2, 1] + "," + this[2, 2] + "," + this[2, 3] + "," + this[2, 4] + "|\n"
                   + "|" + this[3, 1] + "," + this[3, 2] + "," + this[3, 3] + "," + this[3, 4] + "|\n"
                   + "|" + this[4, 1] + "," + this[4, 2] + "," + this[4, 3] + "," + this[4, 4] + "|";
        }


        public static Matrix4 operator +(Matrix4 m1, Matrix4 m2)
        {
            return m1.Add(m2);
        }

        public static Matrix4 operator +(Matrix4 m, Double d)
        {
            return m.Add(d);
        }

        public static Matrix4 operator -(Matrix4 m1, Matrix4 m2)
        {
            return m1.Subtract(m2);
        }

        public static Matrix4 operator -(Matrix4 m, Double d)
        {
            return m.Subtract(d);
        }

        public static Matrix4 operator *(Matrix4 m1, Matrix4 m2)
        {
            return m1.Multiply(m2);
        }

        public static Matrix4 operator *(Matrix4 m, Double d)
        {
            return m.Multiply(d);
        }

        public static Matrix4 operator *(Double d, Matrix4 m)
        {
            return m.Multiply(d);
        }

        public static Matrix4 operator /(Matrix4 m, Double d)
        {
            return m.Divide(d);
        }

        public override int GetHashCode()
        {
            return (_data != null ? _data.GetHashCode() : 0);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var m = obj as Matrix4;
            if (m == null)
            {
                return false;
            }
            return _data[0, 0].NearlyEquals(m._data[0, 0]) && _data[0, 1].NearlyEquals(m._data[0, 1]) && _data[0, 2].NearlyEquals(m._data[0, 2]) && _data[0, 3].NearlyEquals(m._data[0, 3]) &&
                   _data[1, 0].NearlyEquals(m._data[1, 0]) && _data[1, 1].NearlyEquals(m._data[1, 1]) && _data[1, 2].NearlyEquals(m._data[1, 2]) && _data[1, 3].NearlyEquals(m._data[1, 3]) &&
                   _data[2, 0].NearlyEquals(m._data[2, 0]) && _data[2, 1].NearlyEquals(m._data[2, 1]) && _data[2, 2].NearlyEquals(m._data[2, 2]) && _data[2, 3].NearlyEquals(m._data[2, 3]) &&
                   _data[3, 0].NearlyEquals(m._data[3, 0]) && _data[3, 1].NearlyEquals(m._data[3, 1]) && _data[3, 2].NearlyEquals(m._data[3, 2]) && _data[3, 3].NearlyEquals(m._data[3, 3]);
        }

        public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            var vector31 = (eye - target).Normalize();
            var right = up.CrossProduct(vector31).Normalize();
            var vector32 = vector31.CrossProduct(right).Normalize();
            return new Matrix4(new[,]
                {
                    {right.X, right.Y, right.Z, 0.0},
                    {vector32.X, vector32.Y, vector32.Z, 0.0},
                    {vector31.X, vector31.Y, vector31.Z, 0.0},
                    {0.0, 0.0, 0.0, 1.0}
                }) * Translate(-eye);
        }

        public static Matrix4 CreatePerspective(double fovy, double aspect, double zNear, double zFar)
        {
            if (fovy <= 0.0 || fovy > Math.PI)
                throw new ArgumentOutOfRangeException("fovy");
            if (aspect <= 0.0)
                throw new ArgumentOutOfRangeException("aspect");
            if (zNear <= 0.0)
                throw new ArgumentOutOfRangeException("zNear");
            if (zFar <= 0.0)
                throw new ArgumentOutOfRangeException("zFar");
            if (zNear >= zFar)
                throw new ArgumentOutOfRangeException("zNear");
            var top = zNear * Math.Tan(0.5 * fovy);
            var bottom = -top;
            var left = bottom * aspect;
            var right = top * aspect;
            return new Matrix4(new[,]
                {
                    {2.0*zNear/(right - left), 0.0, (right + left)/(right - left), 0.0},
                    {0.0, 2.0*zNear/(top - bottom), (top + bottom)/(top - bottom), 0.0},
                    {0.0, 0.0, -(zFar + zNear)/(zFar - zNear), -(2.0*zFar*zNear)/(zFar - zNear)},
                    {0.0, 0.0, -1, 0.0}
                });
        }

        public Matrix4 RotationComponent
        {
            get
            {
                return new Matrix4(new[,]{
                    { this[1, 1] , this[1, 2], this[1, 3] , 0  },
                    { this[2, 1] , this[2, 2], this[2, 3] , 0  },
                    { this[3, 1] , this[3, 2], this[3, 3] , 0  },
                    { this[4, 1] , this[4, 2], this[4, 3] , 1  }
               });
            }
        }

        public Matrix4 TranslationComponent
        {
            get { return Translate(X, Y, Z); }
        }

        public static Matrix4 CreateOrthographic(double xMin, double xMax, double yMin, double yMax, double zMin, double zMax)
        {
            return new Matrix4(new[,]
            {
                {2.0f / (xMax - xMin), 0, 0, -((xMax + xMin)/(xMax - xMin))},
                {0, 2.0f / (yMax - yMin), 0, -((yMax + yMin)/(yMax - yMin))},
                {0, 0, -2.0f / (zMax - zMin), -((zMax + zMin)/(zMax - zMin))},
                {0, 0, 0, 1}
            });
        }

        public static Matrix4 CreatePerspectiveFieldOfView(double fovy, double aspect, double zNear, double zFar)
        {
            if (fovy <= 0.0 || fovy > Math.PI)
                throw new ArgumentOutOfRangeException("fovy");
            if (aspect <= 0.0)
                throw new ArgumentOutOfRangeException("aspect");
            if (zNear <= 0.0)
                throw new ArgumentOutOfRangeException("zNear");
            if (zFar <= 0.0)
                throw new ArgumentOutOfRangeException("zFar");
            if (zNear >= zFar)
                throw new ArgumentOutOfRangeException("zNear");
            var top = zNear * Math.Tan(0.5 * fovy);
            var bottom = -top;
            var left = bottom * aspect;
            var right = top * aspect;
            return new Matrix4(new[,]
            {
                {2.0*zNear/(right - left), 0.0, (right + left)/(right - left), 0.0},
                {0.0, 2.0*zNear/(top - bottom), (top + bottom)/(top - bottom), 0.0},
                {0.0, 0.0, -(zFar + zNear)/(zFar - zNear), -(2.0*zFar*zNear)/(zFar - zNear)},
                {0.0, 0.0, -1, 0.0}
            });
        }

    }
}
