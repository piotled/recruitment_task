using Microsoft.JSInterop;

namespace FrontendBlazor.Authentication.Impl;

public class TokenStorage : ITokenStorage
{
    private readonly IJSRuntime jsRuntime;

    public TokenStorage(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task<string> GetToken()
    {
        var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");
        return token ?? "";
    }

    public async Task StoreToken(string token)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", "token", token);
    }
}
