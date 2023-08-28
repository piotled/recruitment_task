using FrontendBlazor;
using FrontendBlazor.Authentication;
using FrontendBlazor.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddHttpClient("RestApi", client => client.BaseAddress = new Uri("https://localhost:7243"));
builder.Services.AddSingleton<ITokenStorage, TokenStorage>();
builder.Services.AddSingleton<UsersRepository>();
builder.Services.AddSingleton<AuthenticationService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
await builder.Build().RunAsync();
