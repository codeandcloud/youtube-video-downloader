namespace YouTubeVideoDownloader.Application.Abstractions;
internal interface IFileService<T> where T : new()
{
    bool DeleteFilesFromFolder(string filePath, string extension = "");
    List<T> GetDataListFromJson(string filePath);
    T GetDataFromJson(string filePath);
}
