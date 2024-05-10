using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Transactions.Commands
{
    public class CreateTransactionCommand : IRequest<TransactionDto>
    {
        public CreateTransactionCommand(decimal amount, DateTime date, string description, int categoryId)
        {
            Amount = amount;
            Date = date;
            Description = description;
            CategoryId = categoryId;
        }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
