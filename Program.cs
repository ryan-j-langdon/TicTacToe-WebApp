using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TicTacToe;
using TicTacToe.Logic;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<AppState>();
builder.Services.AddSingleton<GameRules>();
builder.Services.AddSingleton<Opponent>();
builder.Services.AddSingleton<GameState>();

await builder.Build().RunAsync();
