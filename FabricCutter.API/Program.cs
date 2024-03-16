using FabricCutter.API.Extensions;
using FabricCutter.API.Logic.Pdf;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddLogging();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureAllServices();
builder.Services.ConfigureCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(CorsConfiguratorExtension.CorsPolicyName);
app.UseHttpsRedirection();
app.MapAllEndpint();

app.Run();
