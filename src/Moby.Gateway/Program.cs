using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudiences = new []
            {
                builder.Configuration["Auth0:Gateway"],
                builder.Configuration["Auth0:Products"],
                builder.Configuration["Auth0:Carts"],
            },
            ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}"
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("Moby.Client", config =>
    {
        config
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOcelot();

var app = builder.Build();

app.UseCors("Moby.Client");

await app.UseOcelot();

app.Run();
