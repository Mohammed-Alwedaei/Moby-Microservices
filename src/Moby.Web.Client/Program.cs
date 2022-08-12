using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Moby.Web.Client;
using Moby.Web.Client.Services;
using Moby.Web.Client.Services.IServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IProductService, ProductService>();

SD.ProductsBaseApi = builder.Configuration["ServicesUrls:Products.API"];

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
