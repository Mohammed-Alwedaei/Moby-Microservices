using Moby.Web.Shared;

namespace Moby.Web.Client.Models;

public class HttpRequestModel
{
    public SD.HttpMethodTypes HttpMethodTypes { get; set; } = SD.HttpMethodTypes.GET;

    public string? Url { get; set; }

    public object? Data { get; set; }

    public string? AccessToken { get; set; }
}