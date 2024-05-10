namespace FinTrack.Application.Responses
{
    public class TransactionDto
    {
        public int Id { get; init; }
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Amount: {Amount}, Date: {Date}, Description: {Description}, CategoryId: {CategoryId}";
        }
    }
}
