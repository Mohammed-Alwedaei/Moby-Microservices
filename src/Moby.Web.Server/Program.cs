using Duende.Bff;
using Duende.Bff.Yarp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddBff().AddRemoteApis();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("cookie", options =>
    {
        options.Cookie.Name = "__Host-blazor";
        options.Cookie.SameSite = SameSiteMode.Strict;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:7246";

        options.ClientId = "Mango";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("mango");

        options.MapInboundClaims = false;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
    });

builder.Services.AddUserAccessTokenHttpClient("apiClient", configureClient: client =>
{
    client.BaseAddress = new Uri("https://localhost:7085");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapBffManagementEndpoints();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.MapControllers()
    .RequireAuthorization()
    .AsBffApiEndpoint();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRemoteBffApiEndpoint(
            "/api/products", "https://localhost:7190/api/Products")
        .RequireAccessToken(TokenType.User);

    endpoints.MapRemoteBffApiEndpoint(
            "/api/carts", "https://localhost:7008/api/carts")
        .RequireAccessToken(TokenType.User);

    endpoints.MapRemoteBffApiEndpoint(
            "/api/cartcoupons", "https://localhost:7008/api/cartcoupons")
        .RequireAccessToken(TokenType.User);

    endpoints.MapRemoteBffApiEndpoint(
            "/api/checkouts", "https://localhost:7008/api/checkouts")
        .RequireAccessToken(TokenType.User);

    endpoints.MapRemoteBffApiEndpoint(
            "/api/coupons", "https://localhost:7081/api/coupons")
        .RequireAccessToken(TokenType.User);
});

app.Run();
