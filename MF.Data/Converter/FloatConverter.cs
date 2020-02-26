using System;
using Newtonsoft.Json;

namespace MF.Data.Converter
{

    public class FloatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null && !string.IsNullOrEmpty(reader.Value.ToString()))
                return Convert.ToSingle(reader.Value);
            return 0;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }
    }
}
