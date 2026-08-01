using SkiaSharp;
using System.Text.Json.Serialization;

namespace Checkers.Elements;

public class ElementClass : BaseElement
{
    [JsonPropertyName("oldPoint")]
    public Point OldPoint { get; set; }

    [JsonPropertyName("radius")]
    public float Radius { get; set; }

    public ElementClass(Point point, BoardCordElement cord, float radius, SKColor color)
    {
        Point = point;
        Radius = radius;
        Color = color;
        Cord = cord;
    }
}
