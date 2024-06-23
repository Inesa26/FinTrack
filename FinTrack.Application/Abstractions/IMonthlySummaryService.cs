using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;

namespace FinTrack.Application.Abstractions
{
    public interface IMonthlySummaryService
    {
        Task UpdateMonthlySummary(int accountId, int year, Month month, decimal transactionAmount);
        Task CreateMonthlySummary(int accountId, int year, Month month, decimal transactionAmount);
        Task<List<MonthlySummaryDto>> GetMonthlySummariesForYear(int accountId, int year);
        Task<MonthlySummaryDto?> GetMonthlySummary(int accountId, int year, Month month);
    }
}
