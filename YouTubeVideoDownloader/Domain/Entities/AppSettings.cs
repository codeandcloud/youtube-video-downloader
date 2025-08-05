namespace YouTubeVideoDownloader.Domain.Entities;

public sealed record AppSettings
{
    public string YouTubeApiKey { get; set; } = "";
}