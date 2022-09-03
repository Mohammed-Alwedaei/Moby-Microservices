using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Moby.ServiceBus;
using Moby.Services.ShoppingCart.API.DbContexts;
using Moby.Services.ShoppingCart.API.Repository;
using WatchDog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

/* Open API (Swagger) Configuration Start */

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Please add Bearer [space] token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>
            {
                "Bearer"
            }
        }
    });
});

/* Open API (Swagger) Configuration End */

/* Database (EF6) Dependency Injection (DI) Configuration Start */

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

/* Database (EF6) Dependency Injection (DI) Configuration End */

/* Automapper Configuration Start */

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

/* Automapper Configuration End */

/* Custom Managers Dependency Injection */

builder.Services.AddScoped<ICartManager, CartManager>();
builder.Services.AddScoped<IMessageBusManager, MessageBusManager>();

builder.Services.AddScoped<ICouponManager, CouponManager>();
builder.Services.AddHttpClient<ICouponManager, CouponManager>(c => c.BaseAddress = new Uri
    (builder.Configuration.GetValue<string>("ServicesUrls:CouponService")));

/* Custom Managers Dependency Injection */

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}"
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadAccess", policy =>
        policy.RequireClaim("scope", "read:carts"));
});

builder.Services.AddCors(options =>
    {
        options.AddPolicy("Moby.Client", config =>
        {
            config.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

builder.Services.AddWatchDogServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWatchDogExceptionLogger();

app.UseCors("Moby.Client");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseWatchDog(options =>
{
    options.WatchPageUsername = app.Configuration.GetValue<string>("WatchDog:Username");
    options.WatchPagePassword = app.Configuration.GetValue<string>("WatchDog:Password");
});

app.MapControllers();

app.Run();
