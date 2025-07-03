using AnyAscii;
using YY_Saver.Extensions;

namespace YY_Saver.VideoSaver;

public class PreparerOfPlaceForSave
{
    private readonly string _folderToSave = "YY_Saver";
    private readonly string _videoNamePrefix = "YY_Video";

    public string Prepare(string title, string format)
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
        string dorectoryToSave = Path.Combine(rootDirectory, _folderToSave);

        if (!Directory.Exists(dorectoryToSave))
        {
            Directory.CreateDirectory(dorectoryToSave);
        }

        return dorectoryToSave;
    }

    private string PrepareFilename(string title, string format)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title), $"Argument \"{nameof(title)}\" is null or whitespace.");
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentNullException(nameof(format), $"Argument \"{nameof(format)}\" is null or whitespace.");
        if (format.Contains('.'))
            throw new ArgumentNullException(nameof(format), $"Argument \"{nameof(format)}\" contains dot.");

        string timeMarkForSort = DateTime.Now.ToString("yyyy.dd.MM.HH.mm.ss");
        string correctFilename = title
            .Transliterate()
            .Trim()
            .ReplaceAllExceptLettersAndDigits(to: '_')
            .ConcatToString();

        string preparedFilename = $"{_videoNamePrefix}_{timeMarkForSort}_{correctFilename}.{format}";

        return preparedFilename;
    }
}
