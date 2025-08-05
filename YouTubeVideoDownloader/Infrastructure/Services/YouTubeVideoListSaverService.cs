using System.Text.Json;
using YouTubeVideoDownloader.Application.Abstractions;
using YouTubeVideoDownloader.Domain.Entities;

namespace YouTubeVideoDownloader.Infrastructure.Services;

internal class YouTubeVideoListSaverService: IYouTubeVideoListSaverService
{
    public void CreateVideosMetaJson(List<VideoDetails> videos, string fullPath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(videos, options);

        File.WriteAllText(fullPath, jsonString);

        Console.WriteLine($"Successfully saved {videos.Count} videos to {fullPath}");
    }
}
