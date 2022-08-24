using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Moby.Services.Coupon.API.DbContexts;
using Moby.Services.Coupon.API.Mapper;
using Moby.Services.Coupon.API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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

builder.Services.AddScoped<ICouponManager, CouponManager>();

/* Dependency Injection (DI) Configuration End */

/* Automapper Configuration Start */

var mapper = MapperConfig.RegisterMaps().CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

/* Automapper Configuration End */

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7246/";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", options =>
    {
        options.RequireAuthenticatedUser();
        options.RequireClaim("scope", "mango");
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Moby.Client", config =>
    {
        config.WithOrigins("https://localhost:7018")
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

app.UseAuthorization();

app.MapControllers();

app.Run();
