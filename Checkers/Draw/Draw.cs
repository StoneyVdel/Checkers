using Checkers.Checker;
using SkiaSharp;

namespace Checkers.Draw;

public static class DrawElements
{
    public static void DrawBoardElements(ElementClass element, SKCanvas canvas)
    {
        using (var paint = new SKPaint { Color = element.Color })
        {
            //TODO fix this loop for user side
            for (double y = element.IsUserSide ? element; y < element.Height; y += element.Size)
            {
                for (double x = element.Size; x < element.Width; x += (2 * element.Size))
                {
                    
                    if ((y / element.Size % 2 != 0) && (x / element.Size % 2 != 0) || (element.IsUserSide && (y / element.Size % 2 == 0) && (x / element.Size % 2 == 0)))
                    {
                        x -= element.Size;
                    }

                    switch (element.Form)
                    {
                        case ElementForm.Circle:
                            var checkerX = (float)(x + element.Size / 2);
                            var checkerY = (float)(y + element.Size / 2);
                            var radius = (float)(element.Size / 2 - 5);
                            if (element.IsUserSide)
                            {
                                MainPage.user.AddChecker(new CheckerObject(new Point(checkerX, checkerY), radius, element.Color));
                            }
                            else
                            {
                                MainPage.opponent.AddChecker(new CheckerObject(new Point(checkerX, checkerY), radius, element.Color));
                            }
                            canvas.DrawCircle(checkerX, checkerY, radius, paint);
                            
                            break;
                        case ElementForm.Rect:
                            canvas.DrawRect((float)x, (float)y, element.Size, element.Size, paint);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
