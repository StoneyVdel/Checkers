using SkiaSharp;
using System.Text.Json.Serialization;

namespace Checkers.Elements;

public partial class BaseElement
{
    [JsonPropertyName("point")]
    public Point Point { get; set; }

    [JsonPropertyName("color")]
    public SKColor Color { get; set; } = SKColors.Transparent;

    [JsonPropertyName("cord")]
    public BoardCordElement Cord { get; set; }
}
