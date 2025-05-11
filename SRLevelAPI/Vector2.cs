using System;
using System.IO;

namespace SRL
{
    /// <summary>
    /// Represents a 2D vector as two floating point numbers <c>X</c>, <c>Y</c>.
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(float value)
        {
            X = value;
            Y = value;
        }

        public Vector2(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }

        public void Read(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 other && Equals(other);
        }

        public bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        public void Normalize()
        {
            float normalizer = 1f / Length();
            X *= normalizer;
            Y *= normalizer;
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        public static Vector2 operator *(Vector2 left, Vector2 right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            return left;
        }

        public static Vector2 operator *(Vector2 left, float right)
        {
            left.X *= right;
            left.Y *= right;
            return left;
        }

        public static Vector2 operator *(float left, Vector2 right)
        {
            return right * left;
        }

        public static Vector2 operator /(Vector2 left, Vector2 right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            return left;
        }

        public static Vector2 operator /(Vector2 left, float right)
        {
            float divisor = 1f / right;
            left.X *= divisor;
            left.Y *= divisor;
            return left;
        }

        public static Vector2 operator -(Vector2 vector)
        {
            vector.X = -vector.X;
            vector.Y = -vector.Y;
            return vector;
        }

        public static float Distance(Vector2 p, Vector2 q)
        {
            return (p - q).Length();
        }
        public static float DistanceSquared(Vector2 p, Vector2 q)
        {
            return (p - q).LengthSquared();
        }

        public static float Dot(Vector2 left, Vector2 right)
        {
            return left.X * right.X + left.Y * right.Y;
        }

        public static Vector2 Lerp(Vector2 left, Vector2 right, float r)
        {
            return left + (right - left) * r;
        }

        public static readonly Vector2 Zero = new Vector2(0f, 0f);
        public static readonly Vector2 One = new Vector2(1f, 1f);
        public static readonly Vector2 UnitX = new Vector2(1f, 0f);
        public static readonly Vector2 UnitY = new Vector2(0f, 1f);
    }
}
