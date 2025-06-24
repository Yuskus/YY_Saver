using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExtractor;

namespace YY_Saver
{
    internal class Program
    {
        static void Main()
        {
            // private settings
            EnvSettings.Configure();

            // save
            Console.WriteLine("Enter youtube url:");
            string? url = Console.ReadLine();
            SafetyCall(() => GetVideoFromYoutubeExplode(url).Wait(), "YoutubeExplode");
            SafetyCall(() => GetVideoFromYoutubeExtractor(url), "YoutubeExtractor");
        }

        static void SafetyCall(Action action, string source)
        {
            try { action(); } catch (Exception ex) { Console.WriteLine(source + ":\n" + ex.Message); }
        }

        static async Task GetVideoFromYoutubeExplode(string? url)
        {
            if (url == null) return;

            // get info and video
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(url);
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(url);
            var streamInfo = streamManifest
                .GetVideoStreams()
                .Where(s => s.Container == Container.Mp4)
                .GetWithHighestVideoQuality();

            // set result info
            string title = video.Title;
            string quality = streamInfo.VideoQuality.Label;
            string size = streamInfo.Size.MegaBytes.ToString();
            string path = $"YY_VIDEO_{video.Title.Replace(" ", "_")}_{DateTime.Now:yyyy.dd.MM.HH.mm.ss}";
            string result = "\n" +
                $"\tName: {video.Title}\n" +
                $"\tQuality: {streamInfo.VideoQuality.Label}\n" +
                $"\tTotal size (Mb): {streamInfo.Size.MegaBytes}\n" +
                $"\tFile Path: {path}" +
                $"\n" +
                $"\tDownloading started.\n";

            // downloading
            _ = youtube.Videos.Streams.DownloadAsync(streamInfo, path).AsTask();

            Console.WriteLine(result);
        }

        static void GetVideoFromYoutubeExtractor(string? videoUrl)
        {
            if (videoUrl == null) return;
            var videoInfos = DownloadUrlResolver.GetDownloadUrls(videoUrl);
            var video = videoInfos.Where(x => x.VideoType == VideoType.Mp4).OrderByDescending(x => x.Resolution).First();
            var videoDownloader = new VideoDownloader(video, Path.Combine(Environment.CurrentDirectory, "YYVideo " + video.Title + ".mp4"));
            videoDownloader.DownloadProgressChanged += (sender, e) => { Console.WriteLine($"Downloaded {e.ProgressPercentage}%"); };
            videoDownloader.Execute();
        }
    }
}
