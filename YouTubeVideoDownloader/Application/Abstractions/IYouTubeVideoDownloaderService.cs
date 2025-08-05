using YouTubeVideoDownloader.Domain.Entities;

namespace YouTubeVideoDownloader.Application.Abstractions;

internal interface IYouTubeVideoDownloaderService
{
    Task DownloadVideosAsync(List<VideoDetails> videosData, string filePath);
    Task DownloadVideoAsync(string videoId, string fileName, string filePath);
}
