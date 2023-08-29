using FrontendBlazor;
using FrontendBlazor.Authentication;
using FrontendBlazor.Authentication.Impl;
using FrontendBlazor.Model;
using FrontendBlazor.Model.Impl;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddHttpClient("RestApi", client => client.BaseAddress = new Uri("https://localhost:7243"));
builder.Services.AddSingleton<ITokenStorage, TokenStorage>();
builder.Services.AddSingleton<IUsersDAO, UsersDAO>();
builder.Services.AddSingleton<ICategoriesDAO, CategoriesDAO>();
builder.Services.AddSingleton<IContactsDAO, ContactsDAO>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
await builder.Build().RunAsync();
