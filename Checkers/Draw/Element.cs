using SkiaSharp;

namespace Checkers.Draw;

public class ElementClass
{
    public ElementForm Form { get; set; }

    public SKColor Color { get; set; }

    public float Height { get; set; }

    public float Width { get; set; }  

    public float Size { get; set; }

    public bool IsUserSide { get; set; }

    public ElementClass(ElementForm form, SKColor color, float height, float width, float size, bool isUserSide = false)
    {
        Form = form;
        Color = color;
        Height = height;
        Width = width;
        Size = size;
        IsUserSide = isUserSide;
    }
}
