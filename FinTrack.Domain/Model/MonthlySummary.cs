using FinTrack.Domain.Enum;

namespace FinTrack.Domain.Model
{
    public class MonthlySummary : Entity
    {
        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }
        public int Year { get; set; }
        public Month Month { get; set; }
        public decimal Income { get; set; } = 0.00m;
        public decimal Expenses { get; set; } = 0.00m;
        public decimal Balance { get; set; } = 0.00m;

        public MonthlySummary(int accountId)
        {
            AccountId = accountId;
        }
    }
}