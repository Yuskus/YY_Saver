using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using YY_Saver.ProgressOutput;
using YY_Saver.VideoSaver.Interfaces;

namespace YY_Saver.VideoSaver.SaveVideoByUrl.Explode;

public class YoutubeExplodeSaver : ISaveVideoByUrl
{
    private readonly PreparerOfPlaceForSave _preparer = new();

    public async Task SaveVideo(string? url, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url), $"Argument {nameof(url)} is null or whitespace.");

        try
        {
            await Console.Out.WriteLineAsync("Getting information...");

            var searchingFormat = Container.Mp4;

            // get info and video
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(url, cancellationToken: cancellationToken);
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(url, cancellationToken: cancellationToken);
            var streamInfo = streamManifest
                .GetVideoStreams()
                .Where(s => s.Container == searchingFormat)
                .GetWithHighestVideoQuality();

            // set result info
            string path = _preparer.Prepare(video.Title, searchingFormat);

            await WriteVideoInfo(video, streamInfo, path);

            // downloading
            using var progress = new ConsoleProgress();
            var downloading = youtube.Videos.Streams.DownloadAsync(
                streamInfo: streamInfo,
                filePath: path,
                cancellationToken: cancellationToken,
                progress: progress);

            _ = RemoveByToken(path, cancellationToken);

            await downloading;
            await SuccesfullyExit();
        }
        catch (OperationCanceledException)
        {
            await Console.Out.WriteLineAsync("Operation cancellation requested.");
        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync($"Error:\n {e.Message}");
        }
    }

    private static async Task WriteVideoInfo(Video video, IVideoStreamInfo streamInfo, string path)
    {
        await Console.Out.WriteLineAsync("\n" +
            $"Name: {video.Title}\n" +
            $"Quality: {streamInfo.VideoQuality.Label}\n" +
            $"Total size (Mb): {streamInfo.Size.MegaBytes:F2}\n" +
            $"File Path: {path}\n" +
            $"\n" +
            $"Downloading started.\n");
    }

    private static async Task RemoveByToken(string path, CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                File.Delete(path);
                await Console.Out.WriteLineAsync("\nRemoved.");
                return;
            }

            await Task.Delay(1000, CancellationToken.None);
        }
    }

    private static async Task SuccesfullyExit()
    {
        await Console.Out.WriteLineAsync("\nDownloaded succesfully.");
        await Task.Delay(2000, CancellationToken.None);
        Environment.Exit(0);
    }
}
