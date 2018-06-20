using System;
using Newtonsoft.Json;
using NQRW.Maths;

namespace NQRW.Settings
{
    public class DegreeConverter : JsonConverter
    {
        public override bool CanWrite { get; } = false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.TokenType == JsonToken.Float ? Angle.FromDegrees(double.Parse(reader.Value.ToString())) : null;
        }

        public override bool CanConvert(Type objectType) => true;
    }
}