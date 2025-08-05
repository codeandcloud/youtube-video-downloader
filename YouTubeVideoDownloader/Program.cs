using YouTubeVideoDownloader.Domain.Entities;
using YouTubeVideoDownloader.Infrastructure.Services;

var basePath = AppContext.BaseDirectory.Split(@"\bin")[0];
var outputPath = Path.Combine(basePath, "Output");
var videosPath = Path.Combine(outputPath, "Videos");
var videosMetaPath = Path.Combine(outputPath, "video-meta.json");

var appSettingsPath = Path.Combine(basePath, "config.json");
var appSettingsFile = new FileService<AppSettings>();
var youTubeApiKey = appSettingsFile.GetDataFromJson(appSettingsPath).YouTubeApiKey;

// https://www.youtube.com/@iiLuminary/videos
var channelId = "UCa2LutmrR7J96a_2mOm5LtA"; 
YouTubeVideoListFetcherService videoListerService = new();
var videosMeta = await videoListerService.GetChannelVideosMeta(channelId);

YouTubeVideoListSaverService videoSaverService = new();
videoSaverService.CreateVideosMetaJson(videosMeta, videosMetaPath);

var fileService = new FileService<VideoDetails>();
var videosData = fileService.GetDataListFromJson(videosMetaPath);

// var areVideosDeleted = fileService.DeleteFilesFromFolder(videosPath, "mp4");
YouTubeVideoDownloaderService videoDownloaderService = new();
await videoDownloaderService.DownloadVideosAsync(videosData, videosPath);

Console.WriteLine("Process Completed");
