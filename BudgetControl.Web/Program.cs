using BudgetControl.Web.Components;
using BudgetControl.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Blazor Server
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpClient para API
builder.Services.AddHttpClient("BudgetApi", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiBaseUrl"]!
    );
});

builder.Services.AddScoped<BudgetApiClient>();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// 🔹 ESSENCIAL
app.UseStaticFiles();

app.UseRouting();
app.UseAntiforgery();

// Blazor Server
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
