using YoutubeExplode;
using YoutubeExplode.Converter;
using YouTubeVideoDownloader.Application.Abstractions;
using YouTubeVideoDownloader.Application.Common.Extensions;
using YouTubeVideoDownloader.Domain.Entities;

namespace YouTubeVideoDownloader.Infrastructure.Services;

internal class YouTubeVideoDownloaderService: IYouTubeVideoDownloaderService
{
    public async Task DownloadVideosAsync(List<VideoDetails> videosData, string filePath)
    {
        foreach (var videoData in videosData)
        {
            var fileName = $@"{videoData.Title.Slugify()}.mp4";
            await DownloadVideoAsync(videoData.VideoId, fileName, filePath);
        }
    }


    public async Task DownloadVideoAsync(string videoId, string fileName, string filePath)
    {
        var fullPath = $@"{filePath}\{fileName}";
        var videoUrl = $"https://youtube.com/watch?v={videoId}";
        var client = new YoutubeClient();

        try
        {
            Console.WriteLine($"Started Processing {fileName}");
            await client.Videos.DownloadAsync(
                videoUrl,
                fullPath, 
                config => config
                    .SetContainer("mp4")
                    .SetPreset(ConversionPreset.UltraFast)
            );
            Console.WriteLine($"Downloaded {fileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Processing: {fileName}");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

}
