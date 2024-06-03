using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetAllTransactionsQuery : IRequest<List<TransactionDto>>
    {
        public GetAllTransactionsQuery(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId { get; set; }  

    }
}
