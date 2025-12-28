using BudgetControl.Web.Services;
using BudgetControl.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Razor Components (Blazor Server)
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpClient para API
builder.Services.AddHttpClient("BudgetApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/");
});

builder.Services.AddScoped<BudgetApiClient>();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Blazor Server
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
