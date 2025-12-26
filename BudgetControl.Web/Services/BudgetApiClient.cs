using BudgetControl.Application.DTOs;
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

    }

}
