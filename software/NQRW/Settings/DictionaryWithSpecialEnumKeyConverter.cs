using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using NQRW.Robotics;

namespace NQRW.Settings
{
    public class DictionaryWithSpecialEnumKeyConverter : JsonConverter
    {
        public override bool CanWrite { get; } = false;

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
            
            return (T)Enum.Parse(enumType, str);
        }

        public override bool CanConvert(Type objectType) => true;
    }
}