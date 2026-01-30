using BudgetControl.Application.Abstractions.Clock;

namespace BudgetControl.Application.Infrastructure.Clock
{
    public sealed class SystemClock : IClock
    {
        private static readonly TimeZoneInfo BrazilTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

        public DateOnly Today()
        {
            var utcNow = DateTime.UtcNow;
            var brazilNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, BrazilTimeZone);
            return DateOnly.FromDateTime(brazilNow);
        }
    }
}
