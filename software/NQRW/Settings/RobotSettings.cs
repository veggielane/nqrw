using System;
using System.Collections;
using NQRW.Robotics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NQRW.Maths;

namespace NQRW.Settings
{
    public class RobotSettings
    {
        public BodySettings Body { get; set; }

        public static RobotSettings LoadFromFile(string path)
        {
            return JsonConvert.DeserializeObject<RobotSettings>(File.ReadAllText(path));
        }
    }

    public class BodySettings
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }
        public double E { get; set; }
        public double F { get; set; }
    }

    public class RobotSettingsOld
    {
        public Dictionary<Leg, LegSettings> Legs { get; } = new Dictionary<Leg, LegSettings>();

        public RobotSettingsOld()
        {
            Legs.Add(Leg.LeftFront, LegSettings.Default);
            Legs.Add(Leg.LeftMiddle, LegSettings.Default);
            Legs.Add(Leg.LeftRear, LegSettings.Default);
            Legs.Add(Leg.RightFront, LegSettings.Default);
            Legs.Add(Leg.RightMiddle, LegSettings.Default);
            Legs.Add(Leg.RightRear, LegSettings.Default);
        }
    }

    public class LegSettings
    {
        public double CoxaLength { get; set; }
        public double FemurLength { get; set; }
        public double TibiaLength { get; set; }
        public double TarsusLength { get; set; }

        public Angle CoxaOffset { get; set; } = Angle.Zero;
        public Angle FemurOffset { get; set; } = Angle.Zero;
        public Angle TibiaOffset { get; set; } = Angle.Zero;
        public Angle TarsusOffset { get; set; } = Angle.Zero;

        public bool CoxaInvert { get; set; } = false;
        public bool FemurInvert { get; set; } = false;
        public bool TibiaInvert { get; set; } = false;
        public bool TarsusInvert { get; set; } = false;


        public static LegSettings Default => new LegSettings(20, 77, 73, 90)
        {
            CoxaOffset = Angle.Zero,
            FemurOffset = Angle.FromDegrees(12.0),
            TibiaOffset = Angle.FromDegrees(70.0),
            TarsusOffset = Angle.FromDegrees(40.0)
        };
        public LegSettings(double coxaLength, double femurLength, double tibiaLength, double tarsusLength)
        {
            CoxaLength = coxaLength;
            FemurLength = femurLength;
            TibiaLength = tibiaLength;
            TarsusLength = tarsusLength;
        }
    }

    public class DictionaryWithSpecialEnumKeyConverter : JsonConverter
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var valueType = objectType.GetGenericArguments()[1];
            var intermediateDictionaryType = typeof(Dictionary<,>).MakeGenericType(typeof(string), valueType);
            var intermediateDictionary = (IDictionary)Activator.CreateInstance(intermediateDictionaryType);
            serializer.Populate(reader, intermediateDictionary);

            var finalDictionary = (IDictionary)Activator.CreateInstance(objectType);
            foreach (DictionaryEntry pair in intermediateDictionary)
                finalDictionary.Add(ToEnum<Leg>(pair.Key.ToString()), pair.Value);

            return finalDictionary;
        }

        private T ToEnum<T>(string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }
            return default(T);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
