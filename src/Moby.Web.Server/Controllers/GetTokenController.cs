using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moby.Web.Shared.Models;
using Newtonsoft.Json;

namespace Moby.Web.Server.Controllers;

[ApiController]
[Authorize(Policy = "ReadAccess")]
[Route("/api/[controller]")]
public class GetTokenController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GetTokenController> _logger;

    public GetTokenController(HttpClient httpClient, ILogger<GetTokenController> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet]
    [Route("/api/[controller]/{issueTokenFor}")]
    public async Task<IActionResult> Get(string issueTokenFor)
    {
        _httpClient.BaseAddress = new Uri("https://dev-m11z2sfu.us.auth0.com/oauth/token");
        _logger.LogInformation("GET: The request is to get access token");

        var parameters = new Dictionary<string, string> {
            { "grant_type", _configuration["M2M:Grant_Type"] },
            { "client_id", _configuration["M2M:Client_Id"] },
            { "client_secret", _configuration["M2M:Client_Secret"] }

        };

        parameters.Add("audience", _configuration["Audiences:Gateway"]);

        var content = new FormUrlEncodedContent(parameters.Select(p => new KeyValuePair<string, string>(p.Key, p.Value?.ToString() ?? "")));

        using var request = new HttpRequestMessage(HttpMethod.Post, "oauth/token")
        {
            Content = content
        };

        var response = await _httpClient.SendAsync(request);
        var jsonContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return BadRequest();

        var token = JsonConvert.DeserializeObject<Token>(jsonContent);
        return Ok(token);
    }
}