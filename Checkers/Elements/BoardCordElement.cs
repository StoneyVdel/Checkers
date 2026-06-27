namespace Checkers.Elements;

public partial class BoardCordElement
{
    public int Column { get; set; }

    public int Row { get; set; }

    public BoardCordElement(int col, int row)
    {
        Column = col;
        Row = row;
    }
}
