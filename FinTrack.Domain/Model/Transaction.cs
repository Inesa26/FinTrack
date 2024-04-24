namespace FinTrack.Domain.Model
{
    public class Transaction : Entity
    {
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public int AccountId { get; init; }
        public virtual Account? Account { get; set; }

        public Transaction(int id, int accountId, decimal amount, DateTime date, string description, int categoryId) 
        {
            AccountId = accountId;
            Amount = amount;
            Date = date;
            Description = description;
            CategoryId = categoryId;
        }
    }
}
