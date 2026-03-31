namespace Lexicom.ConsoleApp.Amenities;

public readonly struct WriteLineSegment
{
    public static implicit operator WriteLineSegment(string text) => new(text, null);

    public static implicit operator WriteLineSegment((string text, ConsoleColor color) segment) => new(segment.text, segment.color);

    private WriteLineSegment(
        string text,
        ConsoleColor? color)
    {
        Text = text;
        Color = color;
    }

    public string Text { get; }
    public ConsoleColor? Color { get; }
}
