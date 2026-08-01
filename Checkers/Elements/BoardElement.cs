using Checkers.Checker;
using SkiaSharp;

namespace Checkers.Elements;

public class BoardElement
{
    private readonly int BoardLength = 8;

    public int boardWidth = 640;

    public int boardHeight = 640;

    public int rectWidth => boardWidth / 8;

    public int rectHeight => boardHeight / 8;

    private List<SquareElement> boardRects = new List<SquareElement>();

    public void AddRect(BaseElement baseElement)
    {
        var rect = new SquareElement(baseElement, rectWidth, rectHeight);
        boardRects.Add(rect);
    }

    public void Draw(SKCanvas canvas)
    {
        if (boardRects is not null)
        {
            boardRects.ForEach(x => x.Draw(canvas));
        }
    }

    public void UpdateState(List<Point> points, bool isOpponent = false)
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
        //Debug.WriteLine($"occupied rect ammount {OccupiedNum}");
    }

    public void CheckDiagonals(BasicChecker basic)
    {
        var DiagonalRowList = new Dictionary<string, List<SquareElement>>
        {
            { "TopLeftDiagonal", new List<SquareElement>() },
            { "LowLeftDiagonal", new List<SquareElement>() },
            { "TopRightDiagonal", new List<SquareElement>() },
            { "LowRightDiagonal", new List<SquareElement>() }
        };

        for (var i = 1; i <= basic.element.Cord.Column; i++)
        {
            DiagonalRowList["TopLeftDiagonal"].AddRange(boardRects.Where(x => x.Cord.Column == (basic.element.Cord.Column - i) && x.Cord.Row == (basic.element.Cord.Row - i)));
            DiagonalRowList["LowLeftDiagonal"].AddRange(boardRects.Where(x => x.Cord.Column == (basic.element.Cord.Column - i) && x.Cord.Row == (basic.element.Cord.Row + i)));
        }
        for (var i = 1; i <= BoardLength - basic.element.Cord.Column; i++)
        {
            DiagonalRowList["TopRightDiagonal"].AddRange(boardRects.Where(x => x.Cord.Column == (basic.element.Cord.Column + i) && x.Cord.Row == (basic.element.Cord.Row - i)));
            DiagonalRowList["LowRightDiagonal"].AddRange(boardRects.Where(x => x.Cord.Column == (basic.element.Cord.Column + i) && x.Cord.Row == (basic.element.Cord.Row + i)));
        }

        foreach (var row in DiagonalRowList.Keys)
        {
            var isForward = false;
            if (row == "TopLeftDiagonal" || row == "TopRightDiagonal")
            {
                isForward = true;
            }
            CheckMove(DiagonalRowList[row], basic, isForward);
        }
    }

    private void CheckMove(List<SquareElement> row, BasicChecker basic, bool isForward)
    {
        if(!basic.IsKing)
        {
            if (row.Count > 1 && row[0].IsOccupied && row[1].IsOccupied && !basic.IsUser)
            {
                basic.IsAttacking = true;
                row[1].SetSelectable(true);
            }
            else if (row.Count > 0 && !row[0].IsOccupied && !basic.IsAttacking && isForward)
            {
                row[0].SetSelectable(true);
            }
        }
        else
        {
            var isAnyOccuppied = row.Any(x => x.IsOccupied);

            if (isAnyOccuppied)
            {
                var occupiedSquare = row.FindIndex(x => x.IsOccupied);
                if (row.Count() > occupiedSquare && !row[occupiedSquare + 1].IsOccupied)
                {
                    basic.IsAttacking = true;
                    row[occupiedSquare + 1].SetSelectable(true);
                }
            }
        }
    }

    public (Point?, BoardCordElement?) CheckHoveredSquares(Point point)
    {
        var squareExists = boardRects.Any(x => x.CheckHoveredSquare(point));

        if(squareExists)
        {
            var hoveredSquare = boardRects.Where(x => x.CheckHoveredSquare(point)).Single();
            ClearSelectable();

            var centeredX = hoveredSquare.Point.X + rectWidth / 2;
            var centeredY = hoveredSquare.Point.Y + rectWidth / 2;
            var centeredSquare = new Point(centeredX, centeredY);

            return (centeredSquare, hoveredSquare.Cord);
        }

        return (null, null);
    }

    public void ClearSelectable()
    {
        foreach (var row in boardRects)
        {
            row.SetSelectable(false);
        }
    }
}
