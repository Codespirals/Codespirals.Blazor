using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Codespirals.Blazor.Services;

/// <summary>
/// A service to facilitate browser-user interactions
/// </summary>
public interface IUserInteractionService
{
    /// <summary>
    /// Make an alert displaying the given message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task MakeAlert(string message);
    /// <summary>
    /// Download a data object as a Json file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="name"></param>
    /// <param name="addTimestamp"></param>
    /// <returns></returns>
    public Task DownloadDataAsJson<T>(T data, string name, bool addTimestamp = true);
    /// <summary>
    /// Upload a Json file and convert it to an object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="file"></param>
    /// <param name="maxFileSize"></param>
    /// <returns></returns>
    public Task<T?> ExtractDataFromJsonFile<T>(IBrowserFile file, int maxFileSize = 512000);
}

/// <inheritdoc/>
public class UserInteractionService(IJSRuntime js) : IUserInteractionService
{
    private IJSRuntime _js = js;

    /// <inheritdoc/>
    public async Task MakeAlert(string message)
        => await _js.InvokeVoidAsync("alert", message);

    /// <inheritdoc/>
    public async Task DownloadDataAsJson<T>(T data, string name, bool addTimestamp = true)
    {
        try
        {
            var fileName = $"{name}.json";
            if (addTimestamp)
                fileName = $"{DateTime.Now:yyMMdd_HHmmss}_{fileName}";

            var fileStream = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(data));
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            await _js.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            await MakeAlert($"Failed to download {name}.json");
        }
    }

    /// <inheritdoc/>
    public async Task<T?> ExtractDataFromJsonFile<T>(IBrowserFile file, int maxFileSize = 512000)
    {
        try
        {
            if (!file.ContentType.Contains("json") || !file.Name.EndsWith(".json"))
            {
                await MakeAlert($"File is of the wrong format.");
                return default;
            }
            if (file.Size > maxFileSize)
            {
                await MakeAlert($"File is too big.");
                return default;
            }
            using var memoryStream = new MemoryStream();
            await file.OpenReadStream(maxFileSize).CopyToAsync(memoryStream);
            if (memoryStream is null || !memoryStream.CanRead)
            {
                await MakeAlert($"Failed to read file.");
                return default;
            }
            var bytes = memoryStream.ToArray();
            var data = JsonSerializer.Deserialize<T>(bytes);
            if (data is null)
            {
                await MakeAlert($"File had no data.");
                return default;
            }
            return data;
        }
        catch (Exception e)
        {
            Console.Write(e.Message); 
            await MakeAlert($"Failed to upload {file.Name}");
            return default;
        }
    }
}
