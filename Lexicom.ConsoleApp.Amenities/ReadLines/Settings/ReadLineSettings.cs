namespace Lexicom.ConsoleApp.Amenities.ReadLines.Settings;
public class ReadLineSettings
{
    public ReadLineSettings()
    {
        var copy = Consolex.CopyDefaultReadLineSettings();

        CancelKey = copy.CancelKey;
        DefaultKey = copy.DefaultKey;
        DefaultInput = copy.DefaultInput;
        InitalInput = copy.InitalInput;
        InputColor = copy.InputColor;
    }

    internal ReadLineSettings(
        ConsoleKey? cancelKey, 
        ConsoleKey? defaultKey, 
        string? defaultInput, 
        string? initalInput,
        ConsoleColor? inputColor)
    {
        CancelKey = cancelKey;
        DefaultKey = defaultKey;
        DefaultInput = defaultInput;
        InitalInput = initalInput;
        InputColor = inputColor;
    }

    public ConsoleKey? CancelKey { get; set; }
    public ConsoleKey? DefaultKey { get; set; }
    public string? DefaultInput { get; set; }
    public string? InitalInput { get; set; }
    public ConsoleColor? InputColor { get; set; }
}
