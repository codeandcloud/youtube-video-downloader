using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeVideoDownloader.Application.Contracts;
using YouTubeVideoDownloader.Domain.Entities;

namespace YouTubeVideoDownloader.Infrastructure.Services;

internal sealed class YouTubeVideoListFetcherService : IYouTubeVideoListerService
{
    private string ApiKey { get { return "AIzaSyAq-9EWNMEffjs4GnSv6jrT94yIJdZxVcY"; } }
    private string ApplicationName { get { return "YouTubeDownloader"; } }

    public async Task<List<VideoDetails>> GetChannelVideosMeta(string channelId)
    {
        var videos = new List<VideoDetails>();

        var _service = new YouTubeService(new BaseClientService.Initializer()
        {
            ApiKey = ApiKey,
            ApplicationName = ApplicationName
        });

        // 1. Get the uploads playlist ID from the channel ID
        var channelsListRequest = _service.Channels.List("contentDetails");
        channelsListRequest.Id = channelId;
        var channelsListResponse = await channelsListRequest.ExecuteAsync();

        var uploadsPlaylistId = channelsListResponse.Items[0].ContentDetails.RelatedPlaylists.Uploads;

        // 2. Get the videos from the uploads playlist
        string nextPageToken = "";
        while (nextPageToken != null)
        {
            var playlistItemsListRequest = _service.PlaylistItems.List("snippet");
            playlistItemsListRequest.PlaylistId = uploadsPlaylistId;
            playlistItemsListRequest.MaxResults = 50;
            playlistItemsListRequest.PageToken = nextPageToken;

            var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();

            foreach (var playlistItem in playlistItemsListResponse.Items)
            {
                var snippet = playlistItem.Snippet;
                var video = new VideoDetails()
                {
                    Title = snippet.Title,
                    VideoId = snippet.ResourceId.VideoId,
                    PublishedAt = snippet.PublishedAtDateTimeOffset.HasValue ? snippet.PublishedAtDateTimeOffset.Value.ToString("F") : "",
                    Description = snippet.Description,
                };
                videos.Add(video);
            }

            nextPageToken = playlistItemsListResponse.NextPageToken;
        }

        return videos;
    }
}
