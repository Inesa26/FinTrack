using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Transactions.Commands
{
    public class DeleteTransactionCommand : IRequest<TransactionDto>
    {
        public DeleteTransactionCommand(int transactionId)
        {
            TransactionId = transactionId;
        }

        [Required(ErrorMessage = "Transaction Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "TransactionId must be greater than 0")]
        public int TransactionId { get; set; }
    }
}
