using Microsoft.EntityFrameworkCore;
using Moby.Services.Product.API.DbContexts;
using Moby.Services.Product.API.Mapper;
using Moby.Services.Product.API.Repository;
using Moby.Services.Product.API.Repository.IRepository;
using Microsoft.OpenApi.Models;

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

/* Dependency Injection (DI) Configuration Start */

builder.Services.AddScoped<IProductRepository, ProductRepository>();

/* Dependency Injection (DI) Configuration End */

/* Automapper Configuration Start */

var mapper = MapperConfig.RegisterMaps().CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

/* Automapper Configuration End */

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}"
        };

        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadAccess", policy =>
        policy.RequireClaim("scope", "read:gateway"));

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Moby.Client");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
