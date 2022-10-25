namespace Papin.Utils;

public static class Logger
{
    public static void WriteInfo(string text)
    {
        WriteCustom(ConsoleColor.Green, "INFO", text);
    }

    public static void WriteError(string text)
    {
        WriteCustom(ConsoleColor.Red, "ERROR", text);
    }
    
    private static void WriteCustom(ConsoleColor color, string title, string text)
    {
        Console.BackgroundColor = color;
        Console.Write(" {0} ", title);
        ResetConsole();
        Console.Write(" {0}{1}", text, Environment.NewLine);
    }

    private static void ResetConsole()
    {
        Console.ResetColor();
    }
}