namespace Moby.Web.Shared;

public static class SD
{
    public static string ProductsBaseApi { get; set; }

    public enum HttpMethodTypes
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}