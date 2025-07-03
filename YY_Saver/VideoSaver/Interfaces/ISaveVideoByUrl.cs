namespace YY_Saver.VideoSaver.Interfaces;

public interface ISaveVideoByUrl
{
    Task SaveVideo(string? url, CancellationToken cancellationToken = default);
}
