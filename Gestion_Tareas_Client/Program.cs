using Gestion_Tareas_Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Gestion_Tareas_API.Services.Interfaces;
using Gestion_Tareas_API.Services;
using Gestion_Tareas_Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var url = "https://localhost:7161/";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(url) });
builder.Services.AddScoped<ITaskService, TaskService>();

await builder.Build().RunAsync();
