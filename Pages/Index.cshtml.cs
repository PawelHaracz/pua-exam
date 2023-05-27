using Microsoft.AspNetCore.Mvc.RazorPages;
using WebStorageSample;

namespace PUAExam.Pages;

public class Index : PageModel
{
    private readonly ILogger<Index> _logger;

    public string DisplayWords { get; private set; }

    public Index(ILogger<Index> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        string content = $"Witam na egzaminie! {Environment.GetEnvironmentVariable(Const.STUDENT_NAME)} time: UTC Now: {DateTimeOffset.UtcNow.ToString()}.";

        StorageHelper.UploadBlob(Environment.GetEnvironmentVariable(Const.ENDPOINT_ENV_NAME), Environment.GetEnvironmentVariable(Const.ENDPOINT_ENV_KEY), Const.CONTAINER_NAME, Const.BLOB_NAME, content).Wait();
        DisplayWords = StorageHelper.GetBlob(Environment.GetEnvironmentVariable(Const.ENDPOINT_ENV_NAME),Environment.GetEnvironmentVariable(Const.ENDPOINT_ENV_KEY), Const.CONTAINER_NAME, Const.BLOB_NAME).Result;
    }
}