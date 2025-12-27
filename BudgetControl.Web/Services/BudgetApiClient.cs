using BudgetControl.Api.DTOs;
using BudgetControl.Application.DTOs;
using BudgetControl.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetControl.Web.Services
{
    public sealed class BudgetApiClient
    {
        private readonly HttpClient _http;

        public BudgetApiClient(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("BudgetApi");
        }

        public async Task<IReadOnlyCollection<BudgetCycleListItemDto>> GetAllCycles()
        {
            return await _http.GetFromJsonAsync<IReadOnlyCollection<BudgetCycleListItemDto>>(
                "/api/budget-cycles")
                ?? Array.Empty<BudgetCycleListItemDto>();
        }

        public async Task<Guid> CreateBudgetCycle(CreateBudgetCycleRequest request)
        {
            var response = await _http.PostAsJsonAsync("/api/budget-cycles", request);
            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<BudgetCycleCreatedDto>();
            return created!.Id;
        }

        public async Task<DailyBudgetSummaryDto> GetDailySummary(Guid cycleId)
        {
            return await _http.GetFromJsonAsync<DailyBudgetSummaryDto>(
                $"api/budget-cycles/{cycleId}/daily-summary")
                ?? throw new InvalidOperationException("Summary not found.");
        }

        public async Task<IReadOnlyCollection<BudgetCycleDayDto>> GetDays(Guid cycleId)
        {
            return await _http.GetFromJsonAsync<IReadOnlyCollection<BudgetCycleDayDto>>(
                $"api/budget-cycles/{cycleId}/days")
                ?? Array.Empty<BudgetCycleDayDto>();
        }

        public async Task<IReadOnlyCollection<DayExpenseDto>> GetExpenses(Guid cycleId, DateOnly date)
        {
            var dateStr = date.ToString("yyyy-MM-dd");

            return await _http.GetFromJsonAsync<IReadOnlyCollection<DayExpenseDto>>(
                $"api/budget-cycles/{cycleId}/days/{dateStr}/expenses")
                ?? Array.Empty<DayExpenseDto>();
        }

        public async Task RegisterExpense(RegisterPartialExpenseInput request)
        {
            var response = await _http.PostAsJsonAsync("api/expenses", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(error);
            }
        }

        public async Task CloseCurrentDay(Guid cycleId)
        {
            var response = await _http.PostAsync(
                $"api/budget-cycles/{cycleId}/close-day",
                null);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(error);
            }
        }

        public async Task<IReadOnlyCollection<FundingSourceVm>> GetAllFundingSources()
        {
            return await _http.GetFromJsonAsync<IReadOnlyCollection<FundingSourceVm>>(
                "api/funding-sources")
                ?? Array.Empty<FundingSourceVm>();
        }

        public async Task<Guid> CreateFundingSource(string description)
        {
            var response = await _http.PostAsJsonAsync(
                "api/funding-sources",
                new { Name = description });

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<FundingSourceCreatedDto>();
            return result!.Id;
        }
    }

}
