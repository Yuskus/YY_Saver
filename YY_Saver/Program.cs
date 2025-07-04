using YY_Saver.VideoSaver.SaveVideoByUrl.Explode;

namespace YY_Saver;

public class Program
{
    public static async Task Main()
    {
        // private environments settings
        using var settings = new EnvSettings();
        settings.Configure();

        // create token source
        using var source = new CancellationTokenSource();

        // save video
        string? url = EnterUrl();
        var saver = new YoutubeExplodeSaver();
        _ = saver.SaveVideo(url, source.Token);

        // interrupt if pressed any key
        await PressAnyKeyToInterrupt(source);
    }

    static string? EnterUrl()
    {
        Console.WriteLine("Enter youtube url:");
        string? url = Console.ReadLine();
        return url;
    }

    static async Task PressAnyKeyToInterrupt(CancellationTokenSource source)
    {
        Console.ReadKey(true);
        await source.CancelAsync();
        await Task.Delay(2000, CancellationToken.None);
    }
}