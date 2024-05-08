using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Transactions.Commands
{
    public class DeleteTransactionCommand : IRequest<TransactionDto>
    {
        public DeleteTransactionCommand(int transactionId)
        {
            TransactionId = transactionId;
        }

        public int TransactionId { get; set; }
    }
}
