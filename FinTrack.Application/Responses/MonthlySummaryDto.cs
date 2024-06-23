using FinTrack.Domain.Enum;

namespace FinTrack.Application.Responses
{
    public class MonthlySummaryDto
    {
        public int Year { get; set; }
        public Month Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal Balance { get; set; }
    }
}
