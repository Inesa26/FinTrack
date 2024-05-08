using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Transactions.Commands
{
    public class UpdateTransactionCommand : IRequest<TransactionDto>
    {
        public UpdateTransactionCommand(int transactionId, decimal amount, DateTime date, string description, int categoryId)
        {
            TransactionId = transactionId;
            Amount = amount;
            Date = date;
            Description = description;
            CategoryId = categoryId;
        }

        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

    }
}
