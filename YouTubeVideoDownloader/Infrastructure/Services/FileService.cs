using System.Text.Json;
using YouTubeVideoDownloader.Application.Abstractions;

namespace YouTubeVideoDownloader.Infrastructure.Services;

internal class FileService<T> : IFileService<T> where T : new()
{
    public bool DeleteFilesFromFolder(string folderPath, string extension = "")
    {
        var result = false;
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine($"Error: The folder at {folderPath} was not found.");
        }
        try
        {
            IEnumerable<string> filesToDelete;
            if (string.IsNullOrEmpty(extension))
            {
                filesToDelete = Directory.EnumerateFiles(folderPath);
                Console.WriteLine($"Attempting to delete all files in: {folderPath}");
            }
            else
            {
                string normalizedExtension = extension.StartsWith(".") ? extension : "." + extension;
                filesToDelete = Directory.EnumerateFiles(folderPath, $"*{normalizedExtension}");
                Console.WriteLine($"Attempting to delete files with extension '{normalizedExtension}' in: {folderPath}");
            }

            bool allDeletedSuccessfully = true;
            foreach (string file in filesToDelete)
            {
                try
                {
                    File.Delete(file);
                    Console.WriteLine($"Deleted file: {file}");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Error: Access denied while deleting {file}. {ex.Message}");
                    allDeletedSuccessfully = false;
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error: I/O error deleting {file}. {ex.Message}");
                    allDeletedSuccessfully = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred while deleting {file}: {ex.Message}");
                    allDeletedSuccessfully = false;
                }
            }

            result = allDeletedSuccessfully;
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Error: Access denied to folder {folderPath}. {ex.Message}");
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine($"Error: Directory {folderPath} not found. {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"I/O Error accessing folder {folderPath}: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while enumerating files in {folderPath}: {ex.Message}");
        }
        return result;
    }

    public List<T> GetDataListFromJson(string filePath)
    {
        return ReadAndDeserialize<List<T>>(filePath) ?? new List<T>();
    }

    public T GetDataFromJson(string filePath)
    {
        return ReadAndDeserialize<T>(filePath);
    }

    private TResult ReadAndDeserialize<TResult>(string filePath) where TResult : new()
    {
        TResult result = new();
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: The file at {filePath} was not found.");
            return default!;
        }
        try
        {
            string jsonString = File.ReadAllText(filePath);
            var deserialized = JsonSerializer.Deserialize<TResult>(jsonString);

            if (deserialized == null)
            {
                Console.WriteLine($"Warning: Deserialization resulted in null for file: {filePath}. " +
                                  "This might indicate empty or malformed JSON for the target type.");
            }
            else
            {
                result = deserialized;
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing JSON from {filePath}: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"I/O Error reading file {filePath}: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while processing {filePath}: {ex.Message}");
        }
        return result;
    }
}
