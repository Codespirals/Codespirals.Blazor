using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Codespirals.Blazor.Services;

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
    public Task<T> UploadJsonFile<T>(IBrowserFile file, int maxFileSize = 512000);
}

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
    public async Task<T?> UploadJsonFile<T>(IBrowserFile file, int maxFileSize = 512000)
    {
        try
        {
            if (!file.ContentType.Contains("json"))
                return default;
            using var stream = file.OpenReadStream(maxFileSize);
            if (stream is null)
                return default;
            var data = JsonSerializer.Deserialize<T>(stream);
            if (data is null)
                return default;
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
