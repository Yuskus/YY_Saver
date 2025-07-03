using System.Text;

namespace YY_Saver.ProgressOutput;

internal class ConsoleProgress : IProgress<double>, IDisposable
{
    private readonly char _fillSymbol = '■';
    private readonly char _emptySymbol = '_';

    private readonly StringBuilder _progressLine = new();

    public ConsoleProgress() { }

    public void Report(double progress)
    {
        int fillCount = (int)(progress * 100) / 2;
        int emptyCount = 50 - (int)(progress * 100) / 2;

        _progressLine.Clear();

        _progressLine.Append("Progress: [");
        _progressLine.Append(_fillSymbol, fillCount);
        _progressLine.Append(_emptySymbol, emptyCount);
        _progressLine.Append("] ");
        _progressLine.Append($"{progress:P1}");

        Console.SetCursorPosition(0, Console.CursorTop);

        Console.Write(_progressLine);
    }

    public void Dispose()
    {
        Console.SetCursorPosition(Console.CursorTop, Console.CursorTop);
        _progressLine.Clear();
        Console.WriteLine();
    }
}
