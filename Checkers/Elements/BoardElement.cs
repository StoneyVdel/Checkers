using SkiaSharp;
using System.Diagnostics;

namespace Checkers.Elements;

public class BoardElement
{
    public int boardWidth = 480;

    public int boardHeight = 480;

    public int rectWidth => boardWidth / 8;

    public int rectHeight => boardHeight / 8;

    private List<BoardRect> boardRects = new List<BoardRect>();

    public void AddRect(Point point)
    {
        var rect = new BoardRect(point, rectWidth, rectHeight);
        boardRects.Add(rect);
    }

    public void Draw(SKCanvas canvas)
    {
        if (boardRects is not null)
        {
            boardRects.ForEach(x => x.Draw(canvas));
        }
    }

    public void CheckSelectableRects(Point touch) 
    {
        if (boardRects is not null)
        {
            boardRects.ForEach(x => x.CheckSelectable(touch));
        }
    }

    public void UpdateState(List<Point> points)
    {
        var OccupiedNum = 0;  
        foreach (var rect in boardRects)
        {
            foreach(var point in points)
            {
                if(rect.checkOccupied(point))
                {
                    OccupiedNum++;
                    break;
                }
            }
        }
        Debug.WriteLine($"occupied rect ammount {OccupiedNum}");
    }
}
