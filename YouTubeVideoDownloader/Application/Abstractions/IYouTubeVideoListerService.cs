using YouTubeVideoDownloader.Domain.Entities;

namespace YouTubeVideoDownloader.Application.Contracts;

internal interface IYouTubeVideoListerService
{
    Task<List<VideoDetails>> GetChannelVideosMeta(string channelId);
}
