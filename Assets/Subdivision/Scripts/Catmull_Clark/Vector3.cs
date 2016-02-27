using System;
using System.Collections.Generic;
using System.Linq;

namespace Subdivision.Core
{
    public struct Vector3
    {
        public Vector3(double x, double y, double z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator *(Vector3 v1, double f)
        {
            return new Vector3(v1.X * f, v1.Y * f, v1.Z * f);
        }

        public static Vector3 operator /(Vector3 v1, double f)
        {
            double d = 1 / f;
            return new Vector3(v1.X * d, v1.Y * d, v1.Z * d);
        }

        public static Vector3 operator *(double f, Vector3 v1)
        {
            return new Vector3(v1.X * f, v1.Y * f, v1.Z * f);
        }

        public static Vector3 Max(IEnumerable<Vector3> vectors)
        {
            return
                new Vector3(
                    vectors.Max(v => v.X),
                    vectors.Max(v => v.Y),
                    vectors.Max(v => v.Z));
        }

        public static Vector3 Min(IEnumerable<Vector3> vectors)
        {
            return
                new Vector3(
                    vectors.Min(v => v.X),
                    vectors.Min(v => v.Y),
                    vectors.Min(v => v.Z));
        }

        public static Vector3 Average(IEnumerable<Vector3> vectors)
        {
            return
                new Vector3(
                    vectors.Average(v => v.X),
                    vectors.Average(v => v.Y),
                    vectors.Average(v => v.Z));
        }

        public Vector3 CrossProduct(Vector3 other)
        {
            return
                new Vector3(
                    Y * other.Z - Z * other.Y,
                    Z * other.X - X * other.Z,
                    X * other.Y - Y * other.X);
        }

        public Vector3 Normalized()
        {
            return this / Length();
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Vector3 && Equals((Vector3)obj);
        }

        public bool Equals(Vector3 other)
        {
            const double epsilon = 0.01;
            return
                Math.Abs(X - other.X) < epsilon &&
                Math.Abs(Y - other.Y) < epsilon &&
                Math.Abs(Z - other.Z) < epsilon;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }
}