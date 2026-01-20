using BudgetControl.Domain.Aggregates;
using BudgetControl.Pwa;
using BudgetControl.Pwa.Infrastructure.EventStore;
using BudgetControl.Pwa.Infrastructure.Snapshot;
using BudgetControl.Pwa.Infrastructure.Sync;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ILocalEventStore, InMemoryLocalEventStore>();
builder.Services.AddSingleton<ISyncClient, FakeSyncClient>();
builder.Services.AddScoped<BudgetControl.Pwa.Services.TestEventPipeline>();
builder.Services.AddScoped<BudgetControl.Pwa.Infrastructure.Application.RegisterExpenseUseCase>();
builder.Services.AddSingleton<IAggregateSnapshotStore<BudgetCycle>, InMemoryAggregateSnapshotStore<BudgetCycle>>();
builder.Services.AddScoped<BudgetControl.Pwa.Infrastructure.Application.CreateBudgetCycleUseCase>();


await builder.Build().RunAsync();
