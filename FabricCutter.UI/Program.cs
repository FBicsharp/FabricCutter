using Blazored.Toast;
using FabricCutter.UI;
using FabricCutter.UI.Service;
using FabricCutter.UI.ViewModel;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var configurationService = new UrlBaseSettings();
builder.Configuration.GetSection("UrlBaseSettings").Bind(configurationService);
builder.Services.AddSingleton(configurationService);


builder.Services.AddScoped(sp => new HttpClient 
{
	BaseAddress = new Uri(configurationService.BaseAddress),
	Timeout= TimeSpan.FromSeconds(configurationService.SecondTimeout)  
});
builder.Services.AddScoped<IBracketsStringService, BracketsStringService>();
builder.Services.AddScoped<IBracketsViewModel, BracketsViewModel>();
builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();
