using AnyAscii;
using YoutubeExplode.Videos.Streams;
using YY_Saver.Extensions;

namespace YY_Saver.VideoSaver;

public class PreparerOfPlaceForSave
{
    private readonly string _folderToSave = "YY_Saver";
    private readonly string _videoNamePrefix = "YY_Video";

    public string Prepare(string title, Container format)
    {
        string directory = PrepareDirectory();
        string filename = PrepareFilename(title, format);

        string preparedPath = Path.Combine(directory, filename);
        return preparedPath;
    }

    private string PrepareDirectory()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string rootDirectory = Directory.GetDirectoryRoot(currentDirectory);
        string directoryToSave = Path.Combine(rootDirectory, _folderToSave);

        if (!Directory.Exists(directoryToSave))
        {
            Directory.CreateDirectory(directoryToSave);
        }

        return directoryToSave;
    }

    private string PrepareFilename(string title, Container format)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title), $"Argument \"{nameof(title)}\" is null or whitespace.");

        string timeMarkForSort = DateTime.Now.ToString("yyyy.dd.MM.HH.mm.ss");
        string correctFilename = title
            .Transliterate()
            .Trim()
            .ToLower()
            .ReplaceAllExceptLettersAndDigits(to: '_')
            .ConcatToString();

        string preparedFilename = $"{_videoNamePrefix}_{timeMarkForSort}_{correctFilename}.{format}";

        return preparedFilename;
    }
}
