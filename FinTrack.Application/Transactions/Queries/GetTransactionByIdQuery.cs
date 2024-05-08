using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetTransactionByIdQuery : IRequest<TransactionDto>
    {
        public GetTransactionByIdQuery(int transactionId)
        {
            TransactionId = transactionId;
        }

        public int TransactionId { get; set; }
    }
}
