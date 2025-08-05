using YouTubeVideoDownloader.Domain.Entities;

namespace YouTubeVideoDownloader.Application.Abstractions;

internal interface IYouTubeVideoListSaverService
{
    void CreateVideosMetaJson(List<VideoDetails> videos, string fullPath);
}
