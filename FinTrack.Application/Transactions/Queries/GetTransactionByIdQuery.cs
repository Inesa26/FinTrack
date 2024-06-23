using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Transactions.Queries
{
    public class GetTransactionByIdQuery : IRequest<TransactionDto>
    {
        public GetTransactionByIdQuery(int transactionId)
        {
            TransactionId = transactionId;
        }

        [Required(ErrorMessage = "Transaction ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "TransactionId must be greater than 0")]
        public int TransactionId { get; set; }
    }
}
