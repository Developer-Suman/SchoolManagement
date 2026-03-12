using System.Text.Json;
using System.Text.Json.Serialization;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private static readonly string[] Formats =
    {
        "HH:mm",
        "HH:mm:ss"
    };

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (TimeOnly.TryParseExact(value, Formats, out var time))
        {
            return time;
        }

        throw new FormatException($"Invalid TimeOnly format: {value}");
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("HH:mm:ss"));
    }
}