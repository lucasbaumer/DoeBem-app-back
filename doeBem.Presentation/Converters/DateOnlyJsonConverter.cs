using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace doeBem.Presentation.Converters
{
    public class DateOnlyJsonConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? dateString = reader.GetString();
            if (DateTime.TryParseExact(dateString, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return date;
            }
            throw new JsonException($"Data inválida. Backend espera data no formato: {Format}"); 
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
