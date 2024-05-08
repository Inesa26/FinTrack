using FinTrack.Domain.Model;

namespace FinTrack.Application.Responses
{
    public class TransactionDto
    {
        public int Id { get; init; }
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        public static TransactionDto FromTransaction(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                CategoryId = transaction.CategoryId,
            };
        }

        public override string ToString()
        {
            return $"Id: {Id}, Amount: {Amount}, Date: {Date}, Description: {Description}, CategoryId: {CategoryId}";
        }
    }
}
