using System;
using System.IO;

namespace SRL
{
    /// <summary>
    /// Represents a 3D vector as three floating point numbers <c>X</c>, <c>Y</c>, <c>Z</c>.
    /// </summary>
    public struct Vector3 : IEquatable<Vector3>
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(float value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
        }

        public void Read(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3 other && Equals(other);
        }

        public bool Equals(Vector3 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        public void Normalize()
        {
            float normalizer = 1f / Length();
            X *= normalizer;
            Y *= normalizer;
            Z *= normalizer;
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }

        public static Vector3 operator *(Vector3 left, Vector3 right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            return left;
        }

        public static Vector3 operator *(Vector3 left, float right)
        {
            left.X *= right;
            left.Y *= right;
            left.Z *= right;
            return left;
        }

        public static Vector3 operator *(float left, Vector3 right)
        {
            return right * left;
        }

        public static Vector3 operator /(Vector3 left, Vector3 right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            left.Z /= right.Z;
            return left;
        }

        public static Vector3 operator /(Vector3 left, float right)
        {
            float divisor = 1f / right;
            left.X *= divisor;
            left.Y *= divisor;
            left.Z *= divisor;
            return left;
        }

        public static Vector3 operator -(Vector3 vector)
        {
            vector.X = -vector.X;
            vector.Y = -vector.Y;
            vector.Z = -vector.Z;
            return vector;
        }

        public static float Distance(Vector3 p, Vector3 q)
        {
            return (p - q).Length();
        }
        public static float DistanceSquared(Vector3 p, Vector3 q)
        {
            return (p - q).LengthSquared();
        }

        public static float Dot(Vector3 left, Vector3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

        public static Vector3 Lerp(Vector3 left, Vector3 right, float r)
        {
            return left + (right - left) * r;
        }

        public static readonly Vector3 Zero = new Vector3(0f, 0f, 0f);
        public static readonly Vector3 One = new Vector3(1f, 1f, 1f);
        public static readonly Vector3 UnitX = new Vector3(1f, 0f, 0f);
        public static readonly Vector3 UnitY = new Vector3(0f, 1f, 0f);
        public static readonly Vector3 UnitZ = new Vector3(0f, 0f, 1f);
    }
}
