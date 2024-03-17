using Blazored.Toast;
using FabricCutter.UI;
using FabricCutter.UI.Logic;
using FabricCutter.UI.Logic.Events;
using FabricCutter.UI.Service;
using FabricCutter.UI.ViewModel;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var configurationService = new ApplicationSettings();
builder.Configuration.GetSection("ApplicationSettings").Bind(configurationService);
builder.Services.AddSingleton(configurationService);


builder.Services.AddScoped(sp => new HttpClient 
{
	BaseAddress = new Uri(configurationService.UrlBaseSettings.BaseAddress),
	Timeout= TimeSpan.FromSeconds(configurationService.UrlBaseSettings.SecondTimeout)  
});
builder.Services.AddScoped<IMarkerService, MarkerService>();
builder.Services.AddScoped<IRecipeViewModel, RecipeViewModel>();

builder.Services.AddScoped<ISlider, Slider>();
builder.Services.AddScoped<IRecipeInformation, RecipeInformation> ();
builder.Services.AddScoped<IRecipeDetailJsonFactory, RecipeDetailJsonFactory> ();
builder.Services.AddScoped<IMarkerFactory, MarkerFactory> ();
builder.Services.AddScoped<IMarkersCommandViewModel, MarkersCommandViewModel> ();
builder.Services.AddScoped<IMarkersCommandViewModel, MarkersCommandViewModel> ();
builder.Services.AddScoped<IMarkerFactoryValidator, MarkerFactoryValidator> ();

builder.Services.AddSingleton<IEventHub,EventHub>();
builder.Services.AddSingleton<IMarkerService, MarkerService>();








builder.Services.AddScoped<IRecipe,Recipe>();


builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();
