using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using kreddit_app;
using kreddit_app.Data;
using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Indlæser appsettings.json
var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var appSettingsResponse = await httpClient.GetStreamAsync("appsettings.json");
builder.Configuration.AddJsonStream(appSettingsResponse);

// Konfigurer HttpClient med BaseApiUrl fra konfigurationen
var baseApiUrl = builder.Configuration["ApiSettings:BaseApiUrl"];
builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(baseApiUrl) });

builder.Services.AddSingleton<ApiService>();

await builder.Build().RunAsync();
