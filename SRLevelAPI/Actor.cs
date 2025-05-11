using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using System.Linq;
using System.Reflection;

namespace SRL
{
    /// <summary>
    /// Provides various string conversion methods for actor field values.
    /// </summary>
    public static class StringConversion
    {
        /// <summary>
        /// Converts a string to a string.
        /// </summary>
        /// <param name="value">The value as <c>string</c>.</param>
        /// <returns>The value as <c>string</c>.</returns>
        public static string StringToString(string value)
        {
            return value;
        }

        /// <summary>
        /// Converts a string to a string.
        /// </summary>
        /// <param name="value">The value as <c>string</c>.</param>
        /// <returns>The value as <c>string</c>.</returns>
        public static string StringFromString(string value)
        {
            return value;
        }

        /// <summary>
        /// Converts a boolean to a string.
        /// </summary>
        /// <param name="value">The value as <c>bool</c>.</param>
        /// <returns>The value as <c>string</c>.</returns>
        public static string BoolToString(bool value)
        {
            return value ? "TRUE" : "FALSE";
        }

        /// <summary>
        /// Converts a string to a boolean.
        /// </summary>
        /// <param name="value">The value as <c>string</c>.</param>
        /// <returns>The value as <c>bool</c>.</returns>
        public static bool BoolFromString(string value)
        {
            return value == "TRUE";
        }

        /// <summary>
        /// Converts an integer to a string.
        /// </summary>
        /// <param name="value">The value as <c>int</c>.</param>
        /// <returns>The value as <c>string</c>.</returns>
        public static string IntToString(int value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Converts a string to an integer.
        /// </summary>
        /// <param name="value">The value as <c>string</c>.</param>
        /// <returns>The value as <c>int</c>.</returns>
        public static int IntFromString(string value)
        {
            try
            {
                return int.Parse(value, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return int.MaxValue;
            }
        }

        /// <summary>
        /// Converts a floating point number to a string.
        /// </summary>
        /// <param name="value">The value as <c>float</c>.</param>
        /// <returns>The value as <c>string</c>.</returns>
        public static string FloatToString(float value)
        {
            return Convert.ToString(value, CultureInfo.GetCultureInfo("en-US"));
        }

        /// <summary>
        /// Converts a string to a floating point number.
        /// </summary>
        /// <param name="value">The value as <c>string</c>.</param>
        /// <returns>The value as <c>float</c>.</returns>
        public static float FloatFromString(string value)
        {
            try
            {
                return float.Parse(value, CultureInfo.GetCultureInfo("en-US"));
            }
            catch (Exception)
            {
                return float.NaN;
            }
        }

        /// <summary>
        /// Converts an enumerator to a string.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="value">The value as <c>T</c>.</param>
        /// <param name="enumKeys">The enum keys (each key must correspond to an enum entry by index).</param>
        /// <returns>The value as <c>string</c>.</returns>
        public static string EnumToString<T>(T value, string[] enumKeys)
        {
            return enumKeys[(int)(object)value];
        }

        /// <summary>
        /// Converts a string to an enumerator.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="value">The value as <c>string</c>.</param>
        /// <param name="enumKeys">The enum keys (each key must correspond to an enum entry by index).</param>
        /// <returns>The value as <c>T</c>.</returns>
        public static T EnumFromString<T>(string value, string[] enumKeys)
        {
            return (T)(object)enumKeys.First(k => k == value);
        }

        /// <summary>
        /// Converts a 2D vector to a string.
        /// </summary>
        /// <param name="value">The value as <c>Vector2</c>.</param>
        /// <returns>The value as <c>string</c>.</returns>
        public static string Vector2ToString(Vector2 value)
        {
            return FloatToString(value.X) + "," + FloatToString(value.Y);
        }

        /// <summary>
        /// Converts a string to a 2D vector.
        /// </summary>
        /// <param name="value">The value as <c>string</c>.</param>
        /// <returns>The value as <c>Vector2</c>.</returns>
        public static Vector2 Vector2FromString(string value)
        {
            string[] elems = value.Split(',');
            if (elems.Length != 2)
                return Vector2.Zero;
            return new Vector2(FloatFromString(elems[0]), FloatFromString(elems[1]));
        }

        /// <summary>
        /// Converts a 3D vector to a string.
        /// </summary>
        /// <param name="value">The value as <c>Vector3</c>.</param>
        /// <returns>The value as <c>string</c>.</returns>
        public static string Vector3ToString(Vector3 value)
        {
            return FloatToString(value.X) + "," + FloatToString(value.Y) + "," + FloatToString(value.Z);
        }

        /// <summary>
        /// Converts a string to a 3D Vector.
        /// </summary>
        /// <param name="value">The value as <c>string</c>.</param>
        /// <returns>The value as <c>Vector3</c>.</returns>
        public static Vector3 Vector3FromString(string value)
        {
            string[] elems = value.Split(',');
            if (elems.Length != 3)
                return Vector3.Zero;
            return new Vector3(FloatFromString(elems[0]), FloatFromString(elems[1]), FloatFromString(elems[2]));
        }
    }

    /// <summary>
    /// Describes a field for an actor type by providing a key and default value.
    /// See <c>ActorFieldInfo&lt;T&gt;</c> for also describing typed fields.
    /// </summary>
    public abstract class ActorFieldInfo
    {
        /// <summary>
        /// The field's key.
        /// </summary>
        public readonly string Key;

        /// <summary>
        /// Constructs a new <c>ActorFieldInfo</c> by providing a key.
        /// </summary>
        /// <param name="key">The key.</param>
        public ActorFieldInfo(string key)
        {
            Key = key;
        }

        /// <summary>
        /// The field's default value as a string.
        /// </summary>
        public abstract string DefaultValueString { get; }
    }

    /// <summary>
    /// Describes a field for an actor type by providing a key, default value, value type and methods for string conversions.
    /// See <c>ActorFieldInfo</c> for a typeless version.
    /// </summary>
    /// <typeparam name="T">The type of the field.</typeparam>
    public abstract class ActorFieldInfo<T> : ActorFieldInfo
    {
        /// <summary>
        /// The field's default value as <c>T</c>.
        /// </summary>
        public readonly T Default;

        /// <summary>
        /// Constructs a new <c>ActorFieldInfo&lt;T&gt;</c> by providing a key and a default value as <c>T</c>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="default_">The default value as <c>T</c>.</param>
        public ActorFieldInfo(string key, T default_ = default) :
            base(key)
        {
            Default = default_;
        }

        public override string DefaultValueString => ValueToString(Default);

        /// <summary>
        /// Converts a value from the field's underlying type to a string.
        /// </summary>
        /// <param name="value">The value as the field's underlying type.</param>
        /// <returns>The value as <c>string</c>.</returns>
        public abstract string ValueToString(T value);

        /// <summary>
        /// Converts a string value to the fields underlying type.
        /// </summary>
        /// <param name="value">The value as <c>string</c>.</param>
        /// <returns>The value as the field's underlying type.</returns>
        public abstract T ValueFromString(string value);
    }

    /// <summary>
    /// Describes a string field for an actor type by providing a key, default value and methods for string conversions.
    /// </summary>
    public class ActorFieldInfoString : ActorFieldInfo<string>
    {
        /// <summary>
        /// Constructs a new <c>ActorFieldInfoString</c> by providing a key and a default value as <c>string</c>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="default_">The default value as <c>string</c>.</param>
        public ActorFieldInfoString(string key, string default_ = default) :
            base(key, default_) 
        { }

        public override string ValueToString(string value) 
        {  
            return StringConversion.StringToString(value); 
        }

        public override string ValueFromString(string value)
        {
            return StringConversion.StringFromString(value);
        }
    }

    /// <summary>
    /// Describes a boolean field for an actor type by providing a key, default value and methods for string conversions.
    /// </summary>
    public class ActorFieldInfoBool : ActorFieldInfo<bool>
    {
        /// <summary>
        /// Constructs a new <c>ActorFieldInfoBool</c> by providing a key and a default value as <c>bool</c>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="default_">The default value as <c>bool</c>.</param>
        public ActorFieldInfoBool(string key, bool default_ = default) :
            base(key, default_)
        { }

        public override string ValueToString(bool value)
        {
            return StringConversion.BoolToString(value);
        }

        public override bool ValueFromString(string value)
        {
            return StringConversion.BoolFromString(value);
        }
    }

    /// <summary>
    /// Describes an integer field for an actor type by providing a key, default value and methods for string conversions.
    /// </summary>
    public class ActorFieldInfoInt : ActorFieldInfo<int>
    {
        /// <summary>
        /// Constructs a new <c>ActorFieldInfoInt</c> by providing a key and a default value as <c>int</c>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="default_">The default value as <c>int</c>.</param>
        public ActorFieldInfoInt(string key, int default_ = default) :
            base(key, default_)
        { }

        public override string ValueToString(int value)
        {
            return StringConversion.IntToString(value);
        }

        public override int ValueFromString(string value)
        {
            return StringConversion.IntFromString(value);
        }
    }

    /// <summary>
    /// Describes a floating point number field for an actor type by providing a key, default value and methods for string conversions.
    /// </summary>
    public class ActorFieldInfoFloat : ActorFieldInfo<float>
    {
        /// <summary>
        /// Constructs a new <c>ActorFieldInfoFloat</c> by providing a key and a default value as <c>float</c>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="default_">The default value as <c>float</c>.</param>
        public ActorFieldInfoFloat(string key, float default_ = default) :
            base(key, default_)
        { }

        public override string ValueToString(float value)
        {
            return StringConversion.FloatToString(value);
        }

        public override float ValueFromString(string value)
        {
            return StringConversion.FloatFromString(value);
        }
    }

    /// <summary>
    /// Describes an enumeration field for an actor type by providing a key, enum keys, default value and methods for string conversions.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    public class ActorFieldInfoEnum<T> : ActorFieldInfo<T> where T : Enum
    {
        /// <summary>
        /// The enum keys.
        /// Each key must correspond to an enum entry by index.
        /// </summary>
        public string[] EnumKeys;

        /// <summary>
        /// Constructs a new <c>ActorFieldInfoEnum&lt;T&gt;</c> by providing a key, enum keys and a default value as <c>T</c>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="enumKeys">The enum keys (each key must correspond to an enum entry by index).</param>
        /// <param name="default_">The default value as <c>T</c>.</param>
        public ActorFieldInfoEnum(string key, string[] enumKeys, T default_ = default) :
            base(key, default_)
        { 
            EnumKeys = enumKeys;
        }

        public override string ValueToString(T value)
        {
            return StringConversion.EnumToString(value, EnumKeys);
        }

        public override T ValueFromString(string value)
        {
            return StringConversion.EnumFromString<T>(value, EnumKeys);
        }
    }

    /// <summary>
    /// Describes a 2D vector field for an actor type by providing a key, default value and methods for string conversions.
    /// </summary>
    public class ActorFieldInfoVector2 : ActorFieldInfo<Vector2>
    {
        /// <summary>
        /// Constructs a new <c>ActorFieldInfoVector2</c> by providing a key and a default value as <c>Vector2</c>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="default_">The default value as <c>Vector2</c>.</param>
        public ActorFieldInfoVector2(string key, Vector2 default_ = default) :
            base(key, default_)
        { }

        public override string ValueToString(Vector2 value)
        {
            return StringConversion.Vector2ToString(value);
        }

        public override Vector2 ValueFromString(string value)
        {
            return StringConversion.Vector2FromString(value);
        }
    }

    /// <summary>
    /// Describes a 3D vector field for an actor type by providing a key, default value and methods for string conversions.
    /// </summary>
    public class ActorFieldInfoVector3 : ActorFieldInfo<Vector3>
    {
        /// <summary>
        /// Constructs a new <c>ActorFieldInfoBool</c> by providing a key and a default value as <c>Vector3</c>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="default_">The default value as <c>Vector3</c>.</param>
        public ActorFieldInfoVector3(string key, Vector3 default_ = default) :
            base(key, default_)
        { }

        public override string ValueToString(Vector3 value)
        {
            return StringConversion.Vector3ToString(value);
        }

        public override Vector3 ValueFromString(string value)
        {
            return StringConversion.Vector3FromString(value);
        }
    }

    /// <summary>
    /// Represents an actor's field as a key-value pair, both stored as strings.
    /// </summary>
    public class ActorField
    {
        /// <summary>
        /// The key.
        /// </summary>
        public string Key;

        /// <summary>
        /// The value stored as string.
        /// </summary>
        public string Value;

        /// <summary>
        /// Constructs a new <c>ActorField</c> by providing a key and value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value as string.</param>
        public ActorField(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Constructs a new <c>ActorField</c> that was read from a <c>BinaryReader</c>.
        /// This just calls <c>ReadString</c> two times in order to retrieve the key and value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public ActorField(BinaryReader reader)
        {
            Key = reader.ReadString();
            Value = reader.ReadString();
        }

        /// <summary>
        /// Reads the actor field from a <c>BinaryReader</c>.
        /// This just calls <c>ReadString</c> two times in order to retrieve the key and value.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(BinaryReader reader)
        {
            Key = reader.ReadString();
            Value = reader.ReadString();
        }

        /// <summary>
        /// Writes the actor field to a <c>BinaryWriter</c>.
        /// This just calls <c>Write(string)</c> two times in order to write the key and value.
        /// </summary>
        /// <param name="writer"></param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Key);
            writer.Write(Value);
        }
    }

    /// <summary>
    /// Represents a stateless editable actor as placed and configured in the level editor.
    /// Values are exactly the same and stored in exactly the same way as in the level file format.
    /// An actor contains position and size as 2D vectors, a type as a string ("Pickup", "Obstacle", "Trigger", ...) and
    /// a list of fields as key-value pairs (see <c>ActorField</c>).
    /// As these fields are stored as strings, a <c>TypedActor</c> subclass is provided in order
    /// to make accessing and modifying these values easier.
    /// See <c>TypedActor</c>.
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// The actor's initial position when spawned into the level.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The actor's initial size when spawned into the level.
        /// Some actors ignore this value.
        /// </summary>
        public Vector2 Size;

        /// <summary>
        /// The actor's type as a string.
        /// </summary>
        public string TypeStr;

        /// <summary>
        /// The actor's fields as a list of key-value pairs.
        /// </summary>
        public List<ActorField> Fields = new List<ActorField>();

        /// <summary>
        /// Constructs a new <c>Actor</c>.
        /// This will not initialize any values.
        /// </summary>
        public Actor()
        {

        }

        /// <summary>
        /// Constructs a new <c>Actor</c> by providing a position.
        /// This will not initialize any other values.
        /// </summary>
        /// <param name="position">The actor's position.</param>
        public Actor(Vector2 position)
        {
            Position = position;
        }

        /// <summary>
        /// Constructs a new <c>Actor</c> by providing a position.
        /// This will not initialize any other values.
        /// </summary>
        /// <param name="x">The actor's x-position.</param>
        /// <param name="y">The actor's y-position.</param>
        public Actor(float x, float y) :
            this(new Vector2(x, y))
        {

        }

        /// <summary>
        /// Constructs a new <c>Actor</c> by providing a type and position.
        /// This will not initialize any other values.
        /// </summary>
        /// <param name="typeStr">The actor's type as a string.</param>
        /// <param name="position">The actor's position.</param>
        public Actor(string typeStr, Vector2 position) :
            this()
        {
            Position = position;
            TypeStr = typeStr;
        }

        /// <summary>
        /// Constructs a new <c>Actor</c> by providing a type and position.
        /// This will not initialize any other values.
        /// </summary>
        /// <param name="typeStr">The actor's type as a string.</param>
        /// <param name="x">The actor's x-position.</param>
        /// <param name="y">The actor's y-position.</param>
        public Actor(string typeStr, float x, float y) :
            this(typeStr, new Vector2(x, y))
        {

        }

        /// <summary>
        /// Constructs a new <c>Actor</c> by reading it from a <c>BinaryReader</c>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Actor(BinaryReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// Reads the actor from a <c>BinaryReader</c>.
        /// </summary>
        /// <param name="reader"></param>
        public void Read(BinaryReader reader)
        {
            PreRead(out Vector2 position, out Vector2 size, out string typeStr, reader);
            ReadAfterPre(position, size, typeStr, reader);
        }

        /// <summary>
        /// Reads position, size and typeStr values out of the <c>BinaryReader</c> just as <c>Read</c> does
        /// but it does not read any actor fields.
        /// This method allows you to peek into the type of the upcoming actor in the stream without constructing an <c>Actor</c> object yet.
        /// This method should be followed by a call to <c>ReadAfterPre</c> in order to read the rest.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="typeStr">The type as a string.</param>
        /// <param name="reader">The reader.</param>
        public static void PreRead(out Vector2 position, out Vector2 size, out string typeStr, BinaryReader reader)
        {
            position = new Vector2(reader);
            size = new Vector2(reader);
            typeStr = reader.ReadString();
        }

        /// <summary>
        /// Reads the actor from a <c>BinaryReader</c> after <c>Position</c>, <c>Size</c> and <c>TypeStr</c>
        /// were already consumed from the reader.
        /// These values are instead provided as parameters.
        /// This method will therefore only read the actor's fields.
        /// This method is meant to be called right after <c>PreRead</c>.
        /// </summary>
        /// <param name="position">The actor's position.</param>
        /// <param name="size">The actor's size.</param>
        /// <param name="typeStr">The actor's type as a string.</param>
        /// <param name="reader">The reader.</param>
        public void ReadAfterPre(Vector2 position, Vector2 size, string typeStr, BinaryReader reader)
        {
            Position = position;
            Size = size;
            TypeStr = typeStr;

            int propertiesCount = reader.ReadInt32();
            Fields = new List<ActorField>(propertiesCount);
            for (int i = 0; i < propertiesCount; i++)
            {
                Fields.Add(new ActorField(reader));
            }
        }

        /// <summary>
        /// Write the actor to a <c>BinaryWriter</c>.
        /// </summary>
        /// <param name="writer"></param>
        public void Write(BinaryWriter writer)
        {
            Position.Write(writer);
            Size.Write(writer);
            writer.Write(TypeStr);

            writer.Write(Fields.Count);
            for (int i = 0; i < Fields.Count; i++)
            {
                Fields[i].Write(writer);
            }
        }

        /// <summary>
        /// Sets the string value of the field with matching key.
        /// If no field of matching key is present, a new <c>ActorField</c> will be constructed and added to <c>Fields</c>.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <param name="value">The field's value as <c>string</c>.</param>
        public void SetValueString(string key, string value)
        {
            bool found = false;
            foreach (ActorField field in Fields)
            {
                if (field.Key == key)
                {
                    field.Value = value;
                    found = true; // don't return immediately in case there are multiple fields with same keys
                }
            }

            if (!found)
                Fields.Add(new ActorField(key, value));
        }

        /// <summary>
        /// Gets the string value of the field with matching key.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <returns>The field's value as <c>string</c> or <c>null</c> if no field of matching key is present.</returns>
        public string GetValueString(string key)
        {
            for (int i = 0; i < Fields.Count; i++)
            {
                if (Fields[i].Key == key)
                    return Fields[i].Value;
            }
            return default;
        }

        /// <summary>
        /// Removes the field with matching key.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <returns><c>true</c> if a field of matching key was found and removed.</returns>
        public bool RemoveValue(string key)
        {
            return Fields.RemoveAll(p => p.Key == key) > 0;
        }

        /// <summary>
        /// Sets the <c>T</c> value of the specified field.
        /// If no field of matching key is present, a new <c>ActorField</c> will be constructed and added to <c>Fields</c>.
        /// </summary>
        /// <typeparam name="T">The field's type.</typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value as <c>T</c>.</param>
        public void SetValue<T>(ActorFieldInfo<T> field, T value)
        {
            SetValueString(field.Key, field.ValueToString(value));
        }

        /// <summary>
        /// Gets the <c>T</c> value of the specified field.
        /// </summary>
        /// <typeparam name="T">The field's type.</typeparam>
        /// <param name="field">The field.</param>
        /// <returns>The value as <c>T</c> or <c>default</c> if no field of matching key is present.</returns>
        public T GetValue<T>(ActorFieldInfo<T> field)
        {
            return field.ValueFromString(GetValueString(field.Key));
        }

        /// <summary>
        /// Sets the boolean value of the field with matching key.
        /// If no field of matching key is present, a new <c>ActorField</c> will be constructed and added to <c>Fields</c>.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <param name="value">The field's value as <c>bool</c>.</param>
        public void SetValueBool(string key, bool value)
        {
            SetValueString(key, StringConversion.BoolToString(value));
        }

        /// <summary>
        /// Gets the boolean value of the specified field.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <returns>The value as <c>bool</c> or <c>false</c> if no field of matching key is present.</returns>
        public bool GetValueBool(string key)
        {
            return StringConversion.BoolFromString(GetValueString(key));
        }

        /// <summary>
        /// Sets the integer value of the field with matching key.
        /// If no field of matching key is present, a new <c>ActorField</c> will be constructed and added to <c>Fields</c>.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <param name="value">The field's value as <c>int</c>.</param>
        public void SetValueInt(string key, int value)
        {
            SetValueString(key, StringConversion.IntToString(value));
        }

        /// <summary>
        /// Gets the integer value of the specified field.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <returns>The value as <c>int</c> or <c>false</c> if no field of matching key is present.</returns>
        public int GetValueInt(string key)
        {
            return StringConversion.IntFromString(GetValueString(key));
        }

        /// <summary>
        /// Sets the floating point number value of the field with matching key.
        /// If no field of matching key is present, a new <c>ActorField</c> will be constructed and added to <c>Fields</c>.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <param name="value">The field's value as <c>float</c>.</param>
        public void SetValueFloat(string key, float value)
        {
            SetValueString(key, StringConversion.FloatToString(value));
        }

        /// <summary>
        /// Gets the floating point number value of the specified field.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <returns>The value as <c>float</c> or <c>false</c> if no field of matching key is present.</returns>
        public float GetValueFloat(string key)
        {
            return StringConversion.FloatFromString(GetValueString(key));
        }

        /// <summary>
        /// Sets the enumeration value of the field with matching key.
        /// If no field of matching key is present, a new <c>ActorField</c> will be constructed and added to <c>Fields</c>.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="key">The field's key.</param>
        /// <param name="enumKeys">The enum keys.</param>
        /// <param name="value">The field's value as <c>T</c>.</param>
        public void SetValueEnum<T>(string key, string[] enumKeys, T value) where T : Enum
        {
            SetValueString(key, StringConversion.EnumToString(value, enumKeys));
        }

        /// <summary>
        /// Gets the boolean value of the specified field.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="key">The field's key.</param>
        /// <param name="enumKeys">The enum keys.</param>
        /// <returns>The value as <c>T</c> or <c>false</c> if no field of matching key is present.</returns>
        public T GetValueEnum<T>(string key, string[] enumKeys) where T : Enum
        {
            return StringConversion.EnumFromString<T>(GetValueString(key), enumKeys);
        }

        /// <summary>
        /// Sets the 2D vector value of the field with matching key.
        /// If no field of matching key is present, a new <c>ActorField</c> will be constructed and added to <c>Fields</c>.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <param name="value">The field's value as <c>Vector2</c>.</param>
        public void SetValueVector2(string key, Vector2 value)
        {
            SetValueString(key, StringConversion.Vector2ToString(value));
        }

        /// <summary>
        /// Gets the 2D vector value of the specified field.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <returns>The value as <c>Vector2</c> or <c>false</c> if no field of matching key is present.</returns>
        public Vector2 GetValueVector2(string key)
        {
            return StringConversion.Vector2FromString(GetValueString(key));
        }

        /// <summary>
        /// Sets the 3D vector value of the field with matching key.
        /// If no field of matching key is present, a new <c>ActorField</c> will be constructed and added to <c>Fields</c>.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <param name="value">The field's value as <c>Vector3</c>.</param>
        public void SetValueVector3(string key, Vector3 value)
        {
            SetValueString(key, StringConversion.Vector3ToString(value));
        }

        /// <summary>
        /// Gets the 3D vector value of the specified field.
        /// </summary>
        /// <param name="key">The field's key.</param>
        /// <returns>The value as <c>Vector3</c> or <c>false</c> if no field of matching key is present.</returns>
        public Vector3 GetValueVector3(string key)
        {
            return StringConversion.Vector3FromString(GetValueString(key));
        }

        /// <summary>
        /// Scales the actor around the origin.
        /// If this actor object is not of <c>TypedActor</c>, this method just naively scales both position and size
        /// without respecting any fields or some actor types having a fixed size.
        /// The <c>TypedActor</c> class and its subclasses override this method for more accurate scaling.
        /// </summary>
        /// <param name="scaleX">The scale factor in x-direction.</param>
        /// <param name="scaleY">The scale factor in y-direction.</param>
        public virtual void Scale(float scaleX, float scaleY)
        {
            Vector2 scale = new Vector2(scaleX, scaleY);
            Position *= scale;
            Size *= scale;
        }
    }

    /// <summary>
    /// The <c>TypedActor</c> class is a subclass of the <c>Actor</c> class.
    /// It has subclasses for each actor type, which provide properties for each field for easy access and modification,
    /// without having to provide the exact key string or having to do any manual string conversion for the value.
    /// Each constructor will also automatically set all the defaults for each field, though it is recommended to use
    /// <c>Level.AddActor&lt;T&gt; instead for adding new actors to a level.</c>
    /// These classes can be seen as wrappers around the <c>Actor</c> class, without adding any new data to it.
    /// </summary>
    public abstract class TypedActor : Actor
    {
        /// <summary>
        /// Lists all subclasses of <c>TypedActor</c>.
        /// </summary>
        public static IEnumerable<Type> AllTypes =>
            typeof(Actor).Assembly.GetTypes().Where(t => typeof(TypedActor).IsAssignableFrom(t) && !t.IsAbstract);

        /// <summary>
        /// Lists all subclasses of <c>TypedActor</c> including their actor type string as stored in <c>Actor.TypeStr</c>.
        /// </summary>
        public static IEnumerable<KeyValuePair<string, Type>> AllTypesWithStr =>
            AllTypes.Select(t => new KeyValuePair<string, Type>(t.GetProperty("TypeAsString", BindingFlags.Public | BindingFlags.Static).GetMethod.Invoke(null, null) as string, t));

        private static readonly Dictionary<string, Type> stringToType = AllTypesWithStr.ToDictionary(p => p.Key, p => p.Value);
        private static readonly Dictionary<Type, string> typeToString = AllTypesWithStr.ToDictionary(p => p.Value, p => p.Key);

        private static readonly Dictionary<Type, List<ActorFieldInfo>> fieldsForEachActor =
            AllTypes.Zip(
                AllTypes.
                    Select(
                        t => t.GetFields(BindingFlags.Public | BindingFlags.Static).Select(f => f.GetValue(null)).Where(v => v is ActorFieldInfo).Select(v => v as ActorFieldInfo).ToList()
                    ),
                (t, v) => new KeyValuePair<Type, List<ActorFieldInfo>>(t, v)
                ).ToDictionary(p => p.Key, p => p.Value);

        private static readonly Dictionary<Type, Vector2> defaultSizesForEachActor =
            AllTypes.Zip(
                AllTypes.
                    Select(
                        t => (Vector2)t.GetProperty("DefaultSize", BindingFlags.Public | BindingFlags.Static).GetMethod.Invoke(null, null)
                    ),
                (t, v) => new KeyValuePair<Type, Vector2>(t, v)
                ).ToDictionary(p => p.Key, p => p.Value);

        /// <summary>
        /// Gets the <c>TypedActor</c> subclass for specified actor type string as stored in <c>Actor.TypeStr</c>.
        /// </summary>
        /// <param name="typeStr">The actor type string.</param>
        /// <returns>The <c>TypedActor</c> subclass corresponding to <c>typeStr</c>.</returns>
        public static Type GetTypeFromString(string typeStr)
        {
            if (stringToType.TryGetValue(typeStr, out Type type))
                return type;
            return default;
        }

        /// <summary>
        /// Gets the actor type string as stored in <c>Actor.TypeStr</c> from the specified <c>TypedActor</c> subclass.
        /// </summary>
        /// <param name="actorType">The <c>TypedActor</c> subclass.</param>
        /// <returns>The actor type string corresponding to <c>actorType</c>.</returns>
        public static string GetTypeString(Type actorType)
        {
            if (typeToString.TryGetValue(actorType, out string typeStr))
                return typeStr;
            return default;
        }

        /// <summary>
        /// Gets the actor type string as stored in <c>Actor.TypeStr</c> from the specified <c>TypedActor</c> subclass <c>T</c>.
        /// </summary>
        /// <typeparam name="T">The <c>TypedActor</c> subclass.</typeparam>
        /// <returns>The actor type string corresponding to <c>T</c>.</returns>
        public static string GetTypeString<T>() where T : TypedActor
        {
            return GetTypeString(typeof(T));
        }

        /// <summary>
        /// Gets the default size for an actor type.
        /// The default size is the size an actor has right after placing it in the level editor.
        /// </summary>
        /// <param name="actorType">The actor type.</param>
        /// <returns>The default size.</returns>
        public static Vector2 GetDefaultSize(Type actorType)
        {
            if (defaultSizesForEachActor.TryGetValue(actorType, out Vector2 size))
                return size;
            return Vector2.Zero;
        }

        /// <summary>
        /// Gets the default size for an actor type.
        /// The default size is the size an actor has right after placing it in the level editor.
        /// </summary>
        /// <typeparam name="T">The actor type.</typeparam>
        /// <returns>The default size.</returns>
        public static Vector2 GetDefaultSize<T>() where T : TypedActor
        {
            return GetDefaultSize(typeof(T));
        }

        /// <summary>
        /// Lists all field definitions for an actor type.
        /// </summary>
        /// <param name="actorType">The actor type.</param>
        /// <returns>The field definitions.</returns>
        public static IEnumerable<ActorFieldInfo> GetAllFields(Type actorType)
        {
            if (fieldsForEachActor.TryGetValue(actorType, out List<ActorFieldInfo> fields))
                return fields;
            return default;
        }

        /// <summary>
        /// Lists all field definitions for an actor type by providing its type string as stored in <c>Actor.TypeStr</c>.
        /// </summary>
        /// <param name="typeStr">The actor type string.</param>
        /// <returns>The field definitions.</returns>
        public static IEnumerable<ActorFieldInfo> GetAllFields(string typeStr)
        {
            return GetAllFields(GetTypeFromString(typeStr));
        }

        /// <summary>
        /// Lists all field definitions for an actor type.
        /// </summary>
        /// <typeparam name="T">The actor type.</typeparam>
        /// <returns>The field definitions.</returns>
        public static IEnumerable<ActorFieldInfo> GetAllFields<T>() where T : TypedActor
        {
            return GetAllFields(typeof(T));
        }

        /// <summary>
        /// Creates a new <c>TypedActor</c> from the specified actor type.
        /// The provided type has to be one of the subclasses of <c>TypedActor</c>.
        /// </summary>
        /// <param name="actorType">The actor type.</param>
        /// <returns>The newly created <c>TypedActor</c> or <c>null</c> if specified type is not a subclass of <c>TypedActor</c>.</returns>
        public static TypedActor CreateNew(Type actorType)
        {
            return actorType?.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null)?.Invoke(null) as TypedActor;
        }

        /// <summary>
        /// Creates a new <c>TypedActor</c> from the specified actor type by providing its type string as stored in <c>Actor.TypeStr</c>.
        /// </summary>
        /// <param name="actorType">The actor type string.</param>
        /// <returns>The newly created <c>TypedActor</c> or <c>null</c> if the specified type could not be found.</returns>
        public static TypedActor CreateNew(string typeStr)
        {
            return CreateNew(GetTypeFromString(typeStr));
        }

        /// <summary>
        /// Sets all values and fields of an actor back to their defaults including <c>Size</c>, <c>TypeStr</c> and <c>Fields</c>,
        /// but not <c>Position</c>.
        /// </summary>
        public virtual void SetDefaults()
        {
            Size = GetDefaultSize(GetType());
            TypeStr = GetTypeString(GetType());
            Fields.Clear();
            foreach (ActorFieldInfo field in GetAllFields(GetType()))
            {
                SetValueString(field.Key, field.DefaultValueString);
            }
        }

        /// <summary>
        /// Scales the actor around the origin.
        /// </summary>
        /// <param name="scaleX">The scale factor in x-direction.</param>
        /// <param name="scaleY">The scale factor in y-direction.</param>
        public override void Scale(float scaleX, float scaleY)
        {
            Vector2 scale = new Vector2(scaleX, scaleY);
            if (this is IResizable)
            {
                Position *= scale;
                Size *= scale;
            }
            else
            {
                Position += Size / 2f;
                Position *= scale;
                Position -= Size / 2f;
            }
        }

        /// <summary>
        /// Creates a new <c>TypedActor</c> and initializes all its values and fields to their default.
        /// </summary>
        public TypedActor() : base()
        {
            SetDefaults();
        }
    }

    public interface ITriggerable
    {
        string TriggerID { get; set; }
    }

    public interface IFlippable
    {
        bool Flipped { get; set; }
    }

    public interface ILayerable
    {
        ELayer Layer { get; set; }
    }

    public interface IResizable
    {

    }

    /// <summary>
    /// Actor wrapper that represents a checkpoint.
    /// Implements <c>ITriggerable</c>.
    /// </summary>
    public class Checkpoint : TypedActor, ITriggerable
    {
        public static string TypeAsString => "Checkpoint";
        public static Vector2 DefaultSize => new Vector2(100f, 100f);

        public static readonly ActorFieldInfoInt FieldID = new ActorFieldInfoInt("ID", 0);
        public static readonly ActorFieldInfoFloat FieldWeight = new ActorFieldInfoFloat("Weight", 0);
        public static readonly ActorFieldInfoBool FieldDefaultActive = new ActorFieldInfoBool("DefaultActive", true);
        public static readonly ActorFieldInfoString FieldTriggerID = new ActorFieldInfoString("Trigger ID", "");
        public static readonly ActorFieldInfoBool FieldIsStartpoint = new ActorFieldInfoBool("IsStartpoint", false);
        public static readonly ActorFieldInfoBool FieldIsHelper = new ActorFieldInfoBool("IsHelper", true);

        /// <summary>
        /// The checkpoint ID.
        /// Consider using <c>Level.AddCheckpoint</c> instead of manually assigning these IDs.
        /// </summary>
        public int ID
        {
            get => GetValue(FieldID);
            set => SetValue(FieldID, value);
        }

        public float Weight
        {
            get => GetValue(FieldWeight);
            set => SetValue(FieldWeight, value);
        }

        public bool DefaultActive
        {
            get => GetValue(FieldDefaultActive);
            set => SetValue(FieldDefaultActive, value);
        }

        public string TriggerID
        {
            get => GetValue(FieldTriggerID);
            set => SetValue(FieldTriggerID, value);
        }

        public bool IsStartpoint
        {
            get => GetValue(FieldIsStartpoint);
            set => SetValue(FieldIsStartpoint, value);
        }

        public bool IsHelper
        {
            get => GetValue(FieldIsHelper);
            set => SetValue(FieldIsHelper, value);
        }

        /// <summary>
        /// The checkpoint's successors.
        /// Consider using <c>Level.AddCheckpoint</c> and <c>Level.CheckpointConnect</c> instead to correctly fill this list.
        /// </summary>
        public IEnumerable<int> NextIDs
        {
            get => Fields.Where(f => f.Key.StartsWith("nextID_")).Select(f => GetValueInt(f.Key));
            set
            {
                Fields.RemoveAll(f => f.Key.StartsWith("nextID_"));
                Fields.AddRange(value.Select((v, i) => new ActorField("nextID_" + i, v.ToString())));
            }
        }
    }

    /// <summary>
    /// Actor wrapper that represents a player start (starting point, not to be confused with spawn point).
    /// </summary>
    public class PlayerStart : TypedActor
    {
        public static string TypeAsString => "PlayerStart";
        public static Vector2 DefaultSize => new Vector2(25f, 45f);
    }

    /// <summary>
    /// Actor wrapper that represents a pickup (item box).
    /// </summary>
    public class Pickup : TypedActor
    {
        public static string TypeAsString => "Pickup";
        public static Vector2 DefaultSize => new Vector2(45f, 45f);
    }

    /// <summary>
    /// Actor wrapper that represents a boost section (blue panel that fills up your boost).
    /// Implements <c>IFlippable</c>.
    /// </summary>
    public class BoostSection : TypedActor, IFlippable
    {
        public static string TypeAsString => "BoostSection";
        public static Vector2 DefaultSize => new Vector2(175f, 64f);

        public enum ERotation
        {
            DEGREE_0, DEGREE_45, DEGREE_90, DEGREE_135, DEGREE_180, DEGREE_MINUS_135, DEGREE_MINUS_90, DEGREE_MINUS_45
        }

        public static readonly ActorFieldInfoBool FieldFlipped = new ActorFieldInfoBool("Flipped", false);
        public static readonly ActorFieldInfoEnum<ERotation> FieldRotation = new ActorFieldInfoEnum<ERotation>("Rotation", new[] { "0", "45", "90", "135", "180", "-135", "-90", "-45" }, ERotation.DEGREE_0);

        public bool Flipped
        {
            get => GetValue(FieldFlipped);
            set => SetValue(FieldFlipped, value);
        }

        public ERotation Rotation
        {
            get => GetValue(FieldRotation);
            set => SetValue(FieldRotation, value);
        }

        private static int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        /// <summary>
        /// Sets the rotation by directly providing the angle in degree.
        /// Consider using the <c>Rotation</c> property instead.
        /// <c>degree</c> must be an integer multiple of 45.
        /// </summary>
        /// <param name="degree">The angle in degree.</param>
        public void SetRotation(int degree)
        {
            degree = Mod(degree, 360);
            if (degree > 180)
                degree -= 360;
            switch (degree)
            {
                case 0:
                    Rotation = ERotation.DEGREE_0;
                    break;
                case 45:
                    Rotation = ERotation.DEGREE_45;
                    break;
                case 90:
                    Rotation = ERotation.DEGREE_90;
                    break;
                case 135:
                    Rotation = ERotation.DEGREE_135;
                    break;
                case 180:
                    Rotation = ERotation.DEGREE_180;
                    break;
                case -135:
                    Rotation = ERotation.DEGREE_MINUS_135;
                    break;
                case -90:
                    Rotation = ERotation.DEGREE_MINUS_90;
                    break;
                case -45:
                    Rotation = ERotation.DEGREE_MINUS_45;
                    break;
            }
        }

        /// <summary>
        /// Gets the rotation as an angle in degree.
        /// The angle will be in the interval (-180, 180].
        /// </summary>
        /// <returns>The angle in degree.</returns>
        public int GetRotation()
        {
            switch (Rotation)
            {
                case ERotation.DEGREE_0:
                    return 0;
                case ERotation.DEGREE_45:
                    return 45;
                case ERotation.DEGREE_90:
                    return 90;
                case ERotation.DEGREE_135:
                    return 135;
                case ERotation.DEGREE_180:
                    return 180;
                case ERotation.DEGREE_MINUS_135:
                    return -135;
                case ERotation.DEGREE_MINUS_90:
                    return -90;
                case ERotation.DEGREE_MINUS_45:
                    return -45;
            }
            return 0;
        }
    }

    /// <summary>
    /// Actor wrapper that represents a lethal obstacle (spike).
    /// </summary>
    public class LethalObstacle : TypedActor
    {
        public static string TypeAsString => "LethalObstacle";
        public static Vector2 DefaultSize => new Vector2(8f, 8f);

        public enum EOrientation
        {
            UP, DOWN, LEFT, RIGHT
        }

        public static readonly ActorFieldInfoEnum<EOrientation> FieldOrientation = new ActorFieldInfoEnum<EOrientation>("Orientation", new[] { "UP", "DOWN", "LEFT", "RIGHT" }, EOrientation.UP);

        public EOrientation Orientation
        {
            get => GetValue(FieldOrientation);
            set => SetValue(FieldOrientation, value);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a super boost volume (booster).
    /// Implements <c>IResizable</c>.
    /// </summary>
    public class SuperBoostVolume : TypedActor, IResizable
    {
        public static string TypeAsString => "SuperBoostVolume";
        public static Vector2 DefaultSize => new Vector2(64f, 64f);
        
        public enum EType
        {
            LEFT, LEFT_UP, UP, RIGHT_UP, RIGHT, RIGHT_DOWN, DOWN, LEFT_DOWN
        }

        public static readonly ActorFieldInfoEnum<EType> FieldType = new ActorFieldInfoEnum<EType>("Type", new[] { "LEFT", "LEFT_UP", "UP", "RIGHT_UP", "RIGHT", "RIGHT_DOWN", "DOWN", "LEFT DOWN" }, EType.LEFT);

        public EType Type
        {
            get => GetValue(FieldType);
            set => SetValue(FieldType, value);
        }
    }

    /// <summary>
    /// Actor wrapper that represents an obstacle (box, crate).
    /// </summary>
    public class Obstacle : TypedActor
    {
        public static string TypeAsString => "Obstacle";
        public static Vector2 DefaultSize => new Vector2(16f, 16f);

        public enum EObstacleID
        {
            CRATE_RED_1, CRATE_RED_2, BOX_BLACK, CREATE_RED_3, TRASH_RED, TRASH_BLACK, IDFK, CRATE_RED_4, SUITCASE, CRATE_RED_5, BARREL_BLACK, BARREL_BLACK_SKULL, BARREL_RED_SKULL, BARREL_RED, CRATE_BLACK, BOX_RED, BRICKS_1, BRICKS_2, BRICKS_3, BRICKS_4
        }

        public static readonly ActorFieldInfoEnum<EObstacleID> FieldObstacleID = new ActorFieldInfoEnum<EObstacleID>("ObstacleId", new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19" }, EObstacleID.CRATE_RED_2);

        public EObstacleID ObstacleID
        {
            get => GetValue(FieldObstacleID);
            set => SetValue(FieldObstacleID, value);
        }
    }

    /// <summary>
    /// Actor wrapper that represents an AI volume.
    /// Implements <c>ITriggerable</c>, <c>IResizable</c>.
    /// </summary>
    public class AIVolume : TypedActor, ITriggerable, IResizable
    {
        public static string TypeAsString => "AIVolume";
        public static Vector2 DefaultSize => new Vector2(64f, 64f);

        public enum EType
        {
            LEFT, RIGHT, JUMP, SLIDE, BOOST, GRAPPLE, LASER_LEFT, LASER_RIGHT
        }

        public static readonly ActorFieldInfoEnum<EType> FieldType = new ActorFieldInfoEnum<EType>("Type", new[] { "LEFT", "RIGHT", "JUMP", "SLIDE", "BOOST", "GRAPPLE", "LASER>LEFT", "LASER>RIGHT" }, EType.LEFT);
        public static readonly ActorFieldInfoString FieldTriggerID = new ActorFieldInfoString("TriggerID", "");
        public static readonly ActorFieldInfoBool FieldDefaultActive = new ActorFieldInfoBool("DefaultActive", true);
        public static readonly ActorFieldInfoBool FieldEasyBots = new ActorFieldInfoBool("Easy Bots", true);
        public static readonly ActorFieldInfoBool FieldMediumBots = new ActorFieldInfoBool("Medium Bots", true);
        public static readonly ActorFieldInfoBool FieldHardBots = new ActorFieldInfoBool("Hard Bots", true);
        public static readonly ActorFieldInfoBool FieldUnfairBots = new ActorFieldInfoBool("Unfair Bots", true);

        public EType Type
        {
            get => GetValue(FieldType);
            set => SetValue(FieldType, value);
        }

        public string TriggerID
        {
            get => GetValue(FieldTriggerID);
            set => SetValue(FieldTriggerID, value);
        }

        public bool DefaultActive
        {
            get => GetValue(FieldDefaultActive);
            set => SetValue(FieldDefaultActive, value);
        }

        public bool EasyBots
        {
            get => GetValue(FieldEasyBots);
            set => SetValue(FieldEasyBots, value);
        }

        public bool MediumBots
        {
            get => GetValue(FieldMediumBots);
            set => SetValue(FieldMediumBots, value);
        }

        public bool HardBots
        {
            get => GetValue(FieldHardBots);
            set => SetValue(FieldHardBots, value);
        }

        public bool UnfairBots
        {
            get => GetValue(FieldUnfairBots);
            set => SetValue(FieldUnfairBots, value);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a switch block (gate).
    /// Implements <c>ITriggerable</c>, <c>IFlippable</c>.
    /// </summary>
    public class SwitchBlock : TypedActor, ITriggerable, IFlippable
    {
        public static string TypeAsString => "SwitchBlock";
        public static Vector2 DefaultSize => new Vector2(159.2169f, 32f);

        public static readonly ActorFieldInfoString FieldTriggerID = new ActorFieldInfoString("TriggerId", "");
        public static readonly ActorFieldInfoFloat FieldClosedRotation = new ActorFieldInfoFloat("ClosedRotation", 0f);
        public static readonly ActorFieldInfoFloat FieldOpenRotation = new ActorFieldInfoFloat("OpenRotation", 0f);
        public static readonly ActorFieldInfoFloat FieldRotationSpeed = new ActorFieldInfoFloat("RotationSpeed", 171.8873f);
        public static readonly ActorFieldInfoBool FieldFlipped = new ActorFieldInfoBool("Flipped", false);

        public string TriggerID
        {
            get => GetValue(FieldTriggerID);
            set => SetValue(FieldTriggerID, value);
        }

        public float ClosedRotation
        {
            get => GetValue(FieldClosedRotation);
            set => SetValue(FieldClosedRotation, value);
        }

        public float OpenRotation
        {
            get => GetValue(FieldOpenRotation);
            set => SetValue(FieldOpenRotation, value);
        }

        public float RotationSpeed
        {
            get => GetValue(FieldRotationSpeed);
            set => SetValue(FieldRotationSpeed, value);
        }

        public bool Flipped
        {
            get => GetValue(FieldFlipped);
            set => SetValue(FieldFlipped, value);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            Position *= new Vector2(scaleX, scaleY);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a switch (lever).
    /// Implements <c>ITriggerable</c>.
    /// </summary>
    public class Switch : TypedActor, ITriggerable
    {
        public static string TypeAsString => "Switch";
        public static Vector2 DefaultSize => new Vector2(48f, 48f);

        public enum EOrientation
        {
            UP, DOWN, LEFT, RIGHT
        }

        public static readonly ActorFieldInfoString FieldTriggerID = new ActorFieldInfoString("TriggerId", "");
        public static readonly ActorFieldInfoEnum<EOrientation> FieldOrientation = new ActorFieldInfoEnum<EOrientation>("Orientation", new[] { "UP", "DOWN", "LEFT", "RIGHT" }, EOrientation.UP);

        public string TriggerID
        {
            get => GetValue(FieldTriggerID);
            set => SetValue(FieldTriggerID, value);
        }

        public EOrientation Orientation
        {
            get => GetValue(FieldOrientation);
            set => SetValue(FieldOrientation, value);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a trigger.
    /// Implements <c>ITriggerable</c>, <c>IResizable</c>.
    /// </summary>
    public class Trigger : TypedActor, ITriggerable, IResizable
    {
        public static string TypeAsString => "Trigger";
        public static Vector2 DefaultSize => new Vector2(64f, 64f);

        public enum EMode
        {
            OPEN_BRIEFLY, CLOSE_BRIEFLY, OPEN, CLOSE
        }

        public static readonly ActorFieldInfoString FieldTriggerID = new ActorFieldInfoString("TriggerID", "");
        public static readonly ActorFieldInfoEnum<EMode> FieldMode = new ActorFieldInfoEnum<EMode>("Mode", new[] { "OPEN BRIEFLY", "CLOSE BRIEFLY", "OPEN", "CLOSE" }, EMode.OPEN_BRIEFLY);
        public static readonly ActorFieldInfoBool FieldReTriggerOnRespawn = new ActorFieldInfoBool("ReTrigger on Respawn", false);

        public string TriggerID
        {
            get => GetValue(FieldTriggerID);
            set => SetValue(FieldTriggerID, value);
        }

        public EMode Mode
        {
            get => GetValue(FieldMode);
            set => SetValue(FieldMode, value);
        }

        public bool ReTriggerOnRespawn
        {
            get => GetValue(FieldReTriggerOnRespawn);
            set => SetValue(FieldReTriggerOnRespawn, value);
        }
    }

    /// <summary>
    /// Actor wrapper that represents an editable sound emitter (sound emitter).
    /// Implements <c>ITriggerable</c>.
    /// </summary>
    public class EditableSoundEmitter : TypedActor, ITriggerable
    {
        public static string TypeAsString => "EditableSoundEmitter";
        public static Vector2 DefaultSize => new Vector2(50f, 50f);

        public static readonly ActorFieldInfoString FieldSFX = new ActorFieldInfoString("SFX", Bundles.Game.game_wind_airtime_3.SoundLabel);
        public static readonly ActorFieldInfoBool FieldLooping = new ActorFieldInfoBool("Looping", false);
        public static readonly ActorFieldInfoFloat FieldVolume = new ActorFieldInfoFloat("Volume", 0.5f);
        public static readonly ActorFieldInfoString FieldTriggerID = new ActorFieldInfoString("TriggerId", "");
        public static readonly ActorFieldInfoBool FieldDirectional = new ActorFieldInfoBool("Directional", true);
        public static readonly ActorFieldInfoFloat FieldFadeSpeed = new ActorFieldInfoFloat("Fade Speed", 1f);

        /// <summary>
        /// The sound effect.
        /// Consider using <c>SetSound</c> instead to modify this value.
        /// </summary>
        public string SFX
        {
            get => GetValue(FieldSFX);
            set => SetValue(FieldSFX, value);
        }

        public bool Looping
        {
            get => GetValue(FieldLooping);
            set => SetValue(FieldLooping, value);
        }

        public float Volume
        {
            get => GetValue(FieldVolume);
            set => SetValue(FieldVolume, value);
        }

        public string TriggerID
        {
            get => GetValue(FieldTriggerID);
            set => SetValue(FieldTriggerID, value);
        }

        public bool Directional
        {
            get => GetValue(FieldDirectional);
            set => SetValue(FieldDirectional, value);
        }

        public float FadeSpeed
        {
            get => GetValue(FieldFadeSpeed);
            set => SetValue(FieldFadeSpeed, value);
        }

        /// <summary>
        /// Sets the sound emitter's sound content.
        /// </summary>
        /// <param name="sound">The sound.</param>
        public void SetSound(Sound sound)
        {
            SFX = sound.SoundLabel;
        }
    }

    /// <summary>
    /// Actor wrapper that represents bubbles (bubble spawner).
    /// Implements <c>IResizable</c>.
    /// </summary>
    public class Bubbles : TypedActor, IResizable
    {
        public static string TypeAsString => "Bubbles";
        public static Vector2 DefaultSize => new Vector2(64f, 64f);

        public enum EDirection
        {
            LEFT, LEFT_UP, UP, RIGHT_UP, RIGHT, RIGHT_DOWN, DOWN, LEFT_DOWN
        }

        public static readonly ActorFieldInfoFloat FieldLifeTime = new ActorFieldInfoFloat("LifeTime", 3f);
        public static readonly ActorFieldInfoEnum<EDirection> FieldDefaultDirection = new ActorFieldInfoEnum<EDirection>("DefaultDirection", new[] { "LEFT", "LEFT_UP", "UP", "RIGHT_UP", "RIGHT", "RIGHT_DOWN", "DOWN", "LEFT_DOWN" }, EDirection.LEFT);

        public float LifeTime
        {
            get => GetValue(FieldLifeTime);
            set => SetValue(FieldLifeTime, value);
        }

        public EDirection DefaultDirection
        {
            get => GetValue(FieldDefaultDirection);
            set => SetValue(FieldDefaultDirection, value);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            base.Scale(scaleX, scaleY);

            LifeTime = (float)Math.Sqrt(scaleX * scaleY);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a fall tile (black box, crate you can temporarily stand on).
    /// </summary>
    public class FallTile : TypedActor
    {
        public static string TypeAsString => "FallTile";
        public static Vector2 DefaultSize => new Vector2(32f, 32f);
    }

    /// <summary>
    /// Actor wrapper that represents a laser.
    /// Implements <c>IFlippable</c>.
    /// </summary>
    public class Laser : TypedActor, IFlippable
    {
        public static string TypeAsString => "Laser";
        public static Vector2 DefaultSize => new Vector2(64f, 64f);

        public static readonly ActorFieldInfoBool FieldFlipped = new ActorFieldInfoBool("Flipped", false);
        public static readonly ActorFieldInfoBool FieldRotate = new ActorFieldInfoBool("Rotate", true);
        public static readonly ActorFieldInfoFloat FieldRotation = new ActorFieldInfoFloat("Rotation", 0f);

        public bool Flipped
        {
            get => GetValue(FieldFlipped);
            set => SetValue(FieldFlipped, value);
        }

        public bool Rotate
        {
            get => GetValue(FieldRotate);
            set => SetValue(FieldRotate, value);
        }

        public float Rotation
        {
            get => GetValue(FieldRotation);
            set => SetValue(FieldRotation, value);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a spawn point (respawn point, not to be confused with player start).
    /// Implements <c>IResizable</c>.
    /// </summary>
    public class SpawnPoint : TypedActor, IResizable
    {
        public static string TypeAsString => "SpawnPoint";
        public static Vector2 DefaultSize => new Vector2(25f, 45f);

        public static readonly ActorFieldInfoFloat FieldSpawnPosX = new ActorFieldInfoFloat("SpawnPosX", 0f);
        public static readonly ActorFieldInfoFloat FieldSpawnPosY = new ActorFieldInfoFloat("SpawnPosY", 0f);

        public float SpawnPosX
        {
            get => GetValue(FieldSpawnPosX);
            set => SetValue(FieldSpawnPosX, value);
        }

        public float SpawnPosY
        {
            get => GetValue(FieldSpawnPosY);
            set => SetValue(FieldSpawnPosY, value);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            base.Scale(scaleX, scaleY);

            SpawnPosX *= scaleX;
            SpawnPosY *= scaleY;
        }
    }

    /// <summary>
    /// Actor wrapper that represents a boosta coke.
    /// </summary>
    public class BoostaCoke : TypedActor
    {
        public static string TypeAsString => "BoostaCoke";
        public static Vector2 DefaultSize => new Vector2(45f, 65f);

        public enum EOrientation
        {
            UP, DOWN, LEFT, RIGHT
        }

        public static readonly ActorFieldInfoEnum<EOrientation> FieldOrientation = new ActorFieldInfoEnum<EOrientation>("Orientation", new[] { "UP", "DOWN", "LEFT", "RIGHT" }, EOrientation.UP);

        public EOrientation Orientation
        {
            get => GetValue(FieldOrientation);
            set => SetValue(FieldOrientation, value);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a deco (graphics object).
    /// Implements <c>ITriggerable</c>, <c>IFlippable</c>, <c>ILayerable</c>, <c>IResizable</c>.
    /// </summary>
    public class Deco : TypedActor, ITriggerable, IFlippable, ILayerable, IResizable
    {
        public static string TypeAsString => "Deco";
        public static Vector2 DefaultSize => new Vector2(64f, 64f);

        public enum EAnimationType
        {
            NONE, CLOCKWISE, COUNTER_CLOCKWISE, SPRITESHEET, FROM_TO, HIDE, HIDE_TIMED, SPAWNER
        }

        public static readonly ActorFieldInfoEnum<EBundle> FieldBundle = new ActorFieldInfoEnum<EBundle>("Bundle", Bundles.BundleNames, EBundle.COMMON);
        public static readonly ActorFieldInfoInt FieldFrame = new ActorFieldInfoInt("Frame", 0);
        public static readonly ActorFieldInfoString FieldFrameKey = new ActorFieldInfoString("FrameKey", "");
        public static readonly ActorFieldInfoInt FieldImageID = new ActorFieldInfoInt("Image ID", 0);
        public static readonly ActorFieldInfoEnum<ELayer> FieldLayer = new ActorFieldInfoEnum<ELayer>("Layer", Layers.LayerNames, ELayer.OBJECT_LAYER);
        public static readonly ActorFieldInfoBool FieldFlipped = new ActorFieldInfoBool("Flipped", false);
        public static readonly ActorFieldInfoString FieldTriggerID = new ActorFieldInfoString("TriggerId", "");
        public static readonly ActorFieldInfoEnum<EAnimationType> FieldAnimationType = new ActorFieldInfoEnum<EAnimationType>("AnimationType", new[] { "NONE", "CLOCKWISE", "C-CLOCKWISE", "SPRITESHEET", "FROM-TO", "HIDE", "HIDE-TIMED", "SPAWNER" }, EAnimationType.SPRITESHEET);
        public static readonly ActorFieldInfoInt FieldColorR = new ActorFieldInfoInt("Color R", 255);
        public static readonly ActorFieldInfoInt FieldColorG = new ActorFieldInfoInt("Color G", 255);
        public static readonly ActorFieldInfoInt FieldColorB = new ActorFieldInfoInt("Color B", 255);
        public static readonly ActorFieldInfoFloat FieldRotation = new ActorFieldInfoFloat("Rotation", 0f);
        public static readonly ActorFieldInfoVector2 FieldPivot = new ActorFieldInfoVector2("Pivot", new Vector2(0.5f, 0.5f));
        public static readonly ActorFieldInfoInt FieldAnimateTo = new ActorFieldInfoInt("AnimateTo", 0);
        public static readonly ActorFieldInfoInt FieldAnimateFrom = new ActorFieldInfoInt("AnimateFrom", 0);
        public static readonly ActorFieldInfoBool FieldReverseAnimation = new ActorFieldInfoBool("Reverse Animation", false);
        public static readonly ActorFieldInfoBool FieldVisible = new ActorFieldInfoBool("Visible", true);
        public static readonly ActorFieldInfoFloat FieldSpawnInterval = new ActorFieldInfoFloat("Spawn Interval", 1f);
        public static readonly ActorFieldInfoVector2 FieldVelocity = new ActorFieldInfoVector2("Velocity", new Vector2(100f, 0f));
        public static readonly ActorFieldInfoFloat FieldLifetime = new ActorFieldInfoFloat("Lifetime", 5f);
        public static readonly ActorFieldInfoString FieldImageName = new ActorFieldInfoString("ImageName", "");

        /// <summary>
        /// The bundle which the graphic belongs to.
        /// Consider using <c>SetGraphic</c> instead of directly modifying this value.
        /// </summary>
        public EBundle Bundle
        {
            get => GetValue(FieldBundle);
            set => SetValue(FieldBundle, value);
        }

        /// <summary>
        /// The graphic's index in the atlas for graphics of type <c>SPRITE_ATLAS</c> or <c>MULTI_SPRITE_ATLAS</c>.
        /// Always 0 for other graphic types.
        /// Consider using <c>SetGraphic</c> instead of directly modifying this value.
        /// </summary>
        public int Frame
        {
            get => GetValue(FieldFrame);
            set => SetValue(FieldFrame, value);
        }

        /// <summary>
        /// The graphic's key in the atlas for graphics of type <c>SPRITE_ATLAS</c> or <c>MULTI_SPRITE_ATLAS</c>.
        /// Always "" for other graphic types.
        /// Consider using <c>SetGraphic</c> instead of directly modifying this value.
        /// </summary>
        public string FrameKey
        {
            get => GetValue(FieldFrameKey);
            set => SetValue(FieldFrameKey, value);
        }

        /// <summary>
        /// The graphic's content ID inside the bundle.
        /// This value is ignored on <c>LABORATORY</c> theme.
        /// Consider using <c>SetGraphic</c> instead of directly modifying this value.
        /// </summary>
        public int ImageID
        {
            get => GetValue(FieldImageID);
            set => SetValue(FieldImageID, value);
        }

        public ELayer Layer
        {
            get => GetValue(FieldLayer);
            set => SetValue(FieldLayer, value);
        }

        public bool Flipped
        {
            get => GetValue(FieldFlipped);
            set => SetValue(FieldFlipped, value);
        }

        public string TriggerID
        {
            get => GetValue(FieldTriggerID);
            set => SetValue(FieldTriggerID, value);
        }

        public EAnimationType AnimationType
        {
            get => GetValue(FieldAnimationType);
            set => SetValue(FieldAnimationType, value);
        }

        public int ColorR
        {
            get => GetValue(FieldColorR);
            set => SetValue(FieldColorR, value);
        }

        public int ColorG
        {
            get => GetValue(FieldColorG);
            set => SetValue(FieldColorG, value);
        }

        public int ColorB
        {
            get => GetValue(FieldColorB);
            set => SetValue(FieldColorB, value);
        }

        public float Rotation
        {
            get => GetValue(FieldRotation);
            set => SetValue(FieldRotation, value);
        }

        public Vector2 Pivot
        {
            get => GetValue(FieldPivot);
            set => SetValue(FieldPivot, value);
        }

        public int AnimateTo
        {
            get => GetValue(FieldAnimateTo);
            set => SetValue(FieldAnimateTo, value);
        }

        public int AnimateFrom
        {
            get => GetValue(FieldAnimateFrom);
            set => SetValue(FieldAnimateFrom, value);
        }

        public bool ReverseAnimation
        {
            get => GetValue(FieldReverseAnimation);
            set => SetValue(FieldReverseAnimation, value);
        }

        public bool Visible
        {
            get => GetValue(FieldVisible);
            set => SetValue(FieldVisible, value);
        }

        public float SpawnInterval
        {
            get => GetValue(FieldSpawnInterval);
            set => SetValue(FieldSpawnInterval, value);
        }

        public Vector2 Velocity
        {
            get => GetValue(FieldVelocity);
            set => SetValue(FieldVelocity, value);
        }

        public float Lifetime
        {
            get => GetValue(FieldLifetime);
            set => SetValue(FieldLifetime, value);
        }

        /// <summary>
        /// The graphic's file name without .xnb extension.
        /// This value is ignore on all themes except <c>LABORATORY</c>.
        /// Consider using <c>SetGraphic</c> instead of directly modifying this value.
        /// </summary>
        public string ImageName
        {
            get => GetValue(FieldImageName);
            set => SetValue(FieldImageName, value);
        }

        /// <summary>
        /// Sets the deco's graphic content.
        /// </summary>
        /// <param name="graphic">The graphic.</param>
        public void SetGraphic(Graphic graphic)
        {
            Bundle = Bundles.FromName(graphic.Bundle);
            ImageID = graphic.ID;
            ImageName = graphic.Name;
            Frame = graphic.Frame;
            FrameKey = graphic.FrameKey;
            Size = new Vector2(graphic.Width, graphic.Height);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            base.Scale(scaleX, scaleY);

            Velocity *= new Vector2(scaleX, scaleY);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a trigger saw (saw).
    /// Implements <c>ITriggerable</c>.
    /// </summary>
    public class TriggerSaw : TypedActor, ITriggerable
    {
        public static string TypeAsString => "TriggerSaw";
        public static Vector2 DefaultSize => new Vector2(100f, 100f);

        public static readonly ActorFieldInfoString FieldTriggerID = new ActorFieldInfoString("TriggerID", "");
        public static readonly ActorFieldInfoFloat FieldTriggerOffsetX = new ActorFieldInfoFloat("triggerOffsetX", 0f);
        public static readonly ActorFieldInfoFloat FieldTriggerOffsetY = new ActorFieldInfoFloat("triggerOffsetY", 0f);

        public string TriggerID
        {
            get => GetValue(FieldTriggerID);
            set => SetValue(FieldTriggerID, value);
        }

        public float TriggerOffsetX
        {
            get => GetValue(FieldTriggerOffsetX);
            set => SetValue(FieldTriggerOffsetX, value);
        }

        public float TriggerOffsetY
        {
            get => GetValue(FieldTriggerOffsetY);
            set => SetValue(FieldTriggerOffsetY, value);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            base.Scale(scaleX, scaleY);

            TriggerOffsetX *= scaleX;
            TriggerOffsetY *= scaleY;
        }
    }

    /// <summary>
    /// Actor wrapper that represents a text deco (text object).
    /// </summary>
    public class TextDeco : TypedActor
    {
        public static string TypeAsString => "TextDeco";
        public static Vector2 DefaultSize => new Vector2(0f, 32f);

        public static readonly ActorFieldInfoString FieldText = new ActorFieldInfoString("Text", "");
        public static readonly ActorFieldInfoInt FieldTutorialTextId = new ActorFieldInfoInt("TutorialTextId", -1);

        public string Text
        {
            get => GetValue(FieldText);
            set => SetValue(FieldText, value);
        }

        public int TutorialTextId
        {
            get => GetValue(FieldTutorialTextId);
            set => SetValue(FieldTutorialTextId, value);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a dove (or sometimes a blue train on <c>METRO</c> theme).
    /// Implements <c>ITriggerable</c>, <c>IFlippable</c>, <c>ILayerable</c>.
    /// </summary>
    public class Dove : TypedActor, ITriggerable, IFlippable, ILayerable
    {
        public static string TypeAsString => "Dove";
        public static Vector2 DefaultSize => new Vector2(260f, 260f);

        public enum EType
        {
            BIRD, BLUE_TRAIN
        }

        public enum EDirection
        {
            UP, DOWN, LEFT, RIGHT
        }

        public static readonly ActorFieldInfoEnum<ELayer> FieldLayer = new ActorFieldInfoEnum<ELayer>("Layer", Layers.LayerNames, ELayer.MIDDLE_OBJECT_LAYER);
        public static readonly ActorFieldInfoBool FieldFlipped = new ActorFieldInfoBool("Flipped", false);
        public static readonly ActorFieldInfoBool FieldRandomizeDirection = new ActorFieldInfoBool("Randomize Direction", false);
        public static readonly ActorFieldInfoEnum<EType> FieldType = new ActorFieldInfoEnum<EType>("Type", new[] { "BIRD", "BLUE TRAIN" }, EType.BIRD);
        public static readonly ActorFieldInfoEnum<EDirection> FieldDirection = new ActorFieldInfoEnum<EDirection>("Direction", new[] { "UP", "DOWN", "LEFT", "RIGHT" }, EDirection.UP);
        public static readonly ActorFieldInfoFloat FieldSpeed = new ActorFieldInfoFloat("Speed", 300f);
        public static readonly ActorFieldInfoFloat FieldDistance = new ActorFieldInfoFloat("Distance", 500f);
        public static readonly ActorFieldInfoString FieldTriggerID = new ActorFieldInfoString("TriggerId", "");

        public ELayer Layer
        {
            get => GetValue(FieldLayer);
            set => SetValue(FieldLayer, value);
        }

        public bool Flipped
        {
            get => GetValue(FieldFlipped);
            set => SetValue(FieldFlipped, value);
        }

        public bool RandomizeDirection
        {
            get => GetValue(FieldRandomizeDirection);
            set => SetValue(FieldRandomizeDirection, value);
        }

        public EType Type
        {
            get => GetValue(FieldType);
            set => SetValue(FieldType, value);
        }

        public EDirection Direction
        {
            get => GetValue(FieldDirection);
            set => SetValue(FieldDirection, value);
        }

        public float Speed
        {
            get => GetValue(FieldSpeed);
            set => SetValue(FieldSpeed, value);
        }

        public float Distance
        {
            get => GetValue(FieldDistance);
            set => SetValue(FieldDistance, value);
        }

        public string TriggerID
        {
            get => GetValue(FieldTriggerID);
            set => SetValue(FieldTriggerID, value);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            base.Scale(scaleX, scaleY);

            Speed *= (float)Math.Sqrt(scaleX * scaleY);
            Distance *= (float)Math.Sqrt(scaleX * scaleY);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a bouncepad.
    /// </summary>
    public class Bouncepad : TypedActor
    {
        public static string TypeAsString => "Bouncepad";
        public static Vector2 DefaultSize => new Vector2(112f, 32f);

        public static readonly ActorFieldInfoInt FieldRotation = new ActorFieldInfoInt("Rotation", 0);

        public int Rotation
        {
            get => GetValue(FieldRotation);
            set => SetValue(FieldRotation, value);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a bookcase on <c>LIBRARY</c> theme and a castle wall on <c>MANSION</c> theme (only available on <c>LIBRARY</c> and <c>MANSION</c> theme).
    /// </summary>
    public class Bookcase : TypedActor
    {
        public static string TypeAsString => "Bookcase";
        public static Vector2 DefaultSize => new Vector2(120f, 120f);

        public static readonly ActorFieldInfoInt FieldIndex = new ActorFieldInfoInt("index", 1);
        public static readonly ActorFieldInfoFloat FieldScaleF = new ActorFieldInfoFloat("scale", 1f);

        public int Index
        {
            get => GetValue(FieldIndex);
            set => SetValue(FieldIndex, value);
        }

        public float ScaleF
        {
            get => GetValue(FieldScaleF);
            set => SetValue(FieldScaleF, value);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            base.Scale(scaleX, scaleY);

            ScaleF *= (float)Math.Sqrt(scaleX * scaleY);
        }
    }

    /// <summary>
    /// Actor wrapper that represents leaves (spawns leaves when running through).
    /// </summary>
    public class Leaves : TypedActor
    {
        public static string TypeAsString => "Leaves";
        public static Vector2 DefaultSize => new Vector2(50f, 30f);
    }

    /// <summary>
    /// Actor wrapper that represents a deco light (disco light, only available on <c>NIGHTCLUB</c> theme).
    /// </summary>
    public class DecoLight : TypedActor
    {
        public static string TypeAsString => "DecoLight";
        public static Vector2 DefaultSize => new Vector2(120f, 120f);

        public enum EType
        {
            ONE, TWO
        }

        public enum EColor
        {
            WHITE, YELLOW, RED, GREEN, BLUE
        }

        public static readonly ActorFieldInfoEnum<EType> FieldType = new ActorFieldInfoEnum<EType>("TYPE", new[] { "ONE", "TWO" }, EType.ONE);
        public static readonly ActorFieldInfoEnum<EColor> FieldColor = new ActorFieldInfoEnum<EColor>("COLOR", new[] { "WHITE", "YELLOW", "RED", "GREEN", "BLUE" }, EColor.WHITE);
        public static readonly ActorFieldInfoFloat FieldLowerBound = new ActorFieldInfoFloat("LowerBound", -0.7853982f);
        public static readonly ActorFieldInfoFloat FieldUpperBound = new ActorFieldInfoFloat("UpperBound", 0.7853982f);

        public EType Type
        {
            get => GetValue(FieldType);
            set => SetValue(FieldType, value);
        }

        public EColor Color
        {
            get => GetValue(FieldColor);
            set => SetValue(FieldColor, value);
        }

        public float LowerBound
        {
            get => GetValue(FieldLowerBound);
            set => SetValue(FieldLowerBound, value);
        }

        public float UpperBound
        {
            get => GetValue(FieldUpperBound);
            set => SetValue(FieldUpperBound, value);
        }
    }

    /// <summary>
    /// Actor wrapper that represents a deco glow (glow object, only available on <c>NIGHTCLUB</c> theme).
    /// </summary>
    public class DecoGlow : TypedActor
    {
        public static string TypeAsString => "DecoGlow";
        public static Vector2 DefaultSize => new Vector2(120f, 120f);
    }

    /// <summary>
    /// Actor wrapper that represents a rocket launcher.
    /// </summary>
    public class RocketLauncher : TypedActor
    {
        public static string TypeAsString => "RocketLauncher";
        public static Vector2 DefaultSize => new Vector2(64f, 64f);
    }

    /// <summary>
    /// Actor wrapper that represents a metro tunnel (only available on <c>METRO</c> theme).
    /// </summary>
    public class MetroTunnel : TypedActor
    {
        public static string TypeAsString => "MetroTunnel";
        public static Vector2 DefaultSize => new Vector2(120f, 120f);
    }
}
