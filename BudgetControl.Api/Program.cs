using BudgetControl.Application.Abstractions.Persistence;
using BudgetControl.Application.UseCases.AdjustBudgetCycleCapacity;
using BudgetControl.Application.UseCases.AdjustBudgetCyclePeriod;
using BudgetControl.Application.UseCases.CloseDay;
using BudgetControl.Application.UseCases.CreateBudgetCycle;
using BudgetControl.Application.UseCases.CreateFundingSource;
using BudgetControl.Application.UseCases.GetAllBudgetCycles;
using BudgetControl.Application.UseCases.GetAllFundingSources;
using BudgetControl.Application.UseCases.GetBudgetCycleDays;
using BudgetControl.Application.UseCases.GetBudgetCycleDetails;
using BudgetControl.Application.UseCases.GetDailyBudgetSummary;
using BudgetControl.Application.UseCases.GetDayExpenses;
using BudgetControl.Application.UseCases.RegisterPartialExpense;
using BudgetControl.Infrastructure.EF;
using BudgetControl.Infrastructure.EF.Persistence;
using BudgetControl.Infrastructure.EF.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ============================
// Controllers / API
// ============================
builder.Services.AddControllers();

// ============================
// Swagger
// ============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============================
// Database (EF + PostgreSQL)
// ============================
builder.Services.AddDbContext<BudgetControlDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("BudgetControl.Migrations"));
});

// ============================
// Repositories (DI)
// ============================
builder.Services.AddScoped<IBudgetCycleRepository, EfBudgetCycleRepository>();
builder.Services.AddScoped<IFundingSourceRepository, EfFundingSourceRepository>();

// Use cases
builder.Services.AddScoped<CreateFundingSourceUseCase>();
builder.Services.AddScoped<CreateBudgetCycleUseCase>();
builder.Services.AddScoped<RegisterPartialExpenseUseCase>();
builder.Services.AddScoped<GetDailyBudgetSummaryUseCase>();
builder.Services.AddScoped<CloseDayUseCase>();
builder.Services.AddScoped<GetBudgetCycleDetailsUseCase>();
builder.Services.AddScoped<GetBudgetCycleDaysUseCase>();
builder.Services.AddScoped<GetDayExpensesUseCase>();
builder.Services.AddScoped<GetAllBudgetCyclesUseCase>();
builder.Services.AddScoped<GetAllFundingSourcesUseCase>();
builder.Services.AddScoped<AdjustBudgetCyclePeriodUseCase>();
builder.Services.AddScoped<AdjustBudgetCycleCapacityUseCase>();

builder.WebHost.UseUrls("http://0.0.0.0:5000");

// ============================
// App
// ============================
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
