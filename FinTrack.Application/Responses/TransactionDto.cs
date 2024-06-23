using FinTrack.Domain.Enum;

namespace FinTrack.Application.Responses
{
    public class TransactionDto
    {
        public int Id { get; init; }
        public int AccountId { get; init; }
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public TransactionType TransactionType { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, AccountId: {AccountId}, Amount: {Amount}, " +
                $"Date: {Date}, Description: {Description}," +
                $" CategoryId: {CategoryId}, TransactionType: {TransactionType}";
        }
    }
}
