using Blazored.LocalStorage;
using KeyStone.Web;
using KeyStone.Web.Contracts;
using KeyStone.Web.Middlewares;
using KeyStone.Web.Services;
using KeyStone.Web.StateFactory;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices(options =>
{
    options.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    options.SnackbarConfiguration.PreventDuplicates = true;
    options.SnackbarConfiguration.NewestOnTop = false;
    options.SnackbarConfiguration.ShowCloseIcon = true;
    options.SnackbarConfiguration.VisibleStateDuration = 4000;
    options.SnackbarConfiguration.HideTransitionDuration = 500;
    options.SnackbarConfiguration.ShowTransitionDuration = 500;
    options.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    options.SnackbarConfiguration.BackgroundBlurred = true;
    options.SnackbarConfiguration.MaxDisplayedSnackbars = 3;
});

builder.Services.AddScoped<ToastService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ILocalStorageProvider, LocalStorageService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthStateProvider>();
builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());
builder.Services.AddScoped<BaseService>();

builder.Services.AddScoped<AuthInterceptor>();
builder.Services.AddScoped<HttpClientResponseInterceptor>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddLocalization();
builder.Services.AddTransient<MudLocalizer, ResXMudLocalizer>();
builder.Services.AddTransient<ResXMudLocalizer>();

await builder.Build().RunAsync();
