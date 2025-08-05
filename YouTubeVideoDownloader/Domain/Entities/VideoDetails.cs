namespace YouTubeVideoDownloader.Domain.Entities;

internal sealed record VideoDetails
{
    public string Title { get; set; } = string.Empty;
    public string VideoId { get; set; } = string.Empty;
    public string? PublishedAt { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
};
