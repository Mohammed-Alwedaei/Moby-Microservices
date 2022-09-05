using Microsoft.EntityFrameworkCore;
using Moby.ServiceBus;
using Moby.Services.Email.API.DbContexts;
using Moby.Services.Email.API.Extensions;
using Moby.Services.Email.API.Messaging;
using Moby.Services.Email.API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* Database (EF6) Dependency Injection (DI) Configuration Start */

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddSingleton(new EmailManager(optionsBuilder.Options));

/* Database (EF6) Dependency Injection (DI) Configuration End */

/* Service Bus Messaging Managers Dependency Injection */

builder.Services.AddSingleton<IServiceBusConsumer, ServiceBusConsumer>();

/* Service Bus Messaging Managers Dependency Injection */

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

app.UseServiceBusConsumer();

app.Run();
