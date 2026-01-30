using BudgetControl.Application.Abstractions.Clock;
using BudgetControl.Application.DTOs;
using BudgetControl.Application.Infrastructure.InMemory;
using BudgetControl.Application.UseCases.CreateBudgetCycle;
using BudgetControl.Application.UseCases.GetDailyBudgetSummary;
using BudgetControl.Application.UseCases.RegisterPartialExpense;
using BudgetControl.Domain.Categories;
using BudgetControl.Domain.Entities;

var cycleRepository = new InMemoryBudgetCycleRepository();
var fundingSourceRepository = new InMemoryFundingSourceRepository();
var categoryRepository = new InMemorySpendingCategoryRepository();
var clock = new SystemClockConsole();

// ===== Seed mínimo =====
var fundingSource = FundingSource.Create("Vale Refeição");
await fundingSourceRepository.SaveAsync(fundingSource);

// Categoria padrão global
categoryRepository.Add(SpendingCategory.Default);

// ===== Use Cases =====
var createCycle = new CreateBudgetCycleUseCase(
    cycleRepository,
    fundingSourceRepository);

var registerExpense = new RegisterPartialExpenseUseCase(
    cycleRepository,
    clock);

var getDailySummary = new GetDailyBudgetSummaryUseCase(
    cycleRepository, clock);

// ===== Estado do console =====
Guid? currentCycleId = null;
var userId = Guid.NewGuid();

// ===== Loop principal =====
while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 - Criar ciclo");
    Console.WriteLine("2 - Lançar gasto");
    Console.WriteLine("3 - Ver meta diária");
    Console.WriteLine("4 - Fechar dia");
    Console.WriteLine("0 - Sair");
    Console.Write("> ");

    var option = Console.ReadLine();

    try
    {
        switch (option)
        {
            case "1":
                Console.Write("Total disponível: ");
                var total = decimal.Parse(Console.ReadLine()!);

                Console.Write("Duração em dias: ");
                var days = int.Parse(Console.ReadLine()!);

                var created = await createCycle.ExecuteAsync(
                    new CreateBudgetCycleInput
                    {
                        FundingSourceId = fundingSource.Id,
                        StartDate = DateOnly.FromDateTime(DateTime.Today),
                        EstimatedDurationInDays = days,
                        TotalCapacity = total
                    });

                currentCycleId = created.Id;

                Console.WriteLine($"Ciclo criado (ID: {created.Id})");
                break;

            case "2":
                EnsureCycle(currentCycleId);

                Console.Write("Valor do gasto: ");
                var amount = decimal.Parse(Console.ReadLine()!);

                Console.Write("Descrição: ");
                var description = Console.ReadLine()!;

                await registerExpense.ExecuteAsync(
                    new RegisterPartialExpenseInput
                    {
                        BudgetCycleId = currentCycleId!.Value,
                        Amount = amount,
                        Description = description
                    });

                Console.WriteLine("Gasto registrado no dia corrente.");
                break;

            case "3":
                EnsureCycle(currentCycleId);

                var summary = await getDailySummary.ExecuteAsync(
                    currentCycleId!.Value);

                Console.WriteLine($"[{summary.FundingSourceName}]");
                Console.WriteLine($"Meta diária: {summary.DailyCapacity}");
                Console.WriteLine($"Restante: {summary.RemainingCapacity}");
                Console.WriteLine($"Dias restantes: {summary.RemainingDays}");
                break;

            case "4":
                EnsureCycle(currentCycleId);

                Console.WriteLine("Dia fechado. Próximo dia agora está ativo.");
                break;

            case "0":
                return;

            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

// ===== Helpers =====
static void EnsureCycle(Guid? cycleId)
{
    if (cycleId is null)
        throw new InvalidOperationException("Nenhum ciclo ativo.");
}

public sealed class SystemClockConsole : IClock
{
    public DateOnly Today()
        => DateOnly.FromDateTime(DateTime.Today);
}