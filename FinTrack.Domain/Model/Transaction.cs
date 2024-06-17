using FinTrack.Domain.Enum;

namespace FinTrack.Domain.Model
{
    public class Transaction : Entity
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public TransactionType Type { get; set; }
        public virtual Category? Category { get; set; }
        public int AccountId { get; init; }
        public virtual Account? Account { get; set; }

        public Transaction() { }

        public Transaction(
            int accountId,
            decimal amount,
            DateTime date,
            string description,
            int categoryId,
            TransactionType transactionType)
        {
            AccountId = accountId;
            Amount = amount;
            Date = date;
            Description = description;
            CategoryId = categoryId;
            Type = transactionType;
        }
    }
}
