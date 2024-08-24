using System.Text.Json.Serialization;

namespace JsonLocalizer;

internal class JsonObject
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = null!;

    [JsonPropertyName("value")]
    public string Value { get; set; } = null!;

    public override bool Equals(object obj) => obj is JsonObject json && Key == json.Key;

    public override int GetHashCode() => HashCode.Combine(Key);
}
