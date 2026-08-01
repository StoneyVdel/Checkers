using System.Text.Json.Serialization;

namespace Checkers.Elements;

public partial class BoardCordElement
{
    [JsonPropertyName("column")]
    public int Column { get; set; }

    [JsonPropertyName("row")]
    public int Row { get; set; }

    public BoardCordElement(int column, int row)
    {
        Column = column;
        Row = row;
    }
}
