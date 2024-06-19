using FinTrack.Application.Responses;
using FinTrack.Domain.Enum;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Transactions.Commands
{
    public class UpdateTransactionCommand : IRequest<TransactionDto>
    {
        public UpdateTransactionCommand(int transactionId, decimal amount, DateTime date, string description, int categoryId, TransactionType transactionType)
        {
            TransactionId = transactionId;
            Amount = amount;
            Date = date;
            Description = description;
            CategoryId = categoryId;
            TransactionType = transactionType;
        }

        [Required(ErrorMessage = "TransactionId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "TransactionId must be greater than 0")]
        public int TransactionId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "TransactionType is required")]
        public TransactionType TransactionType { get; set; }
    }
}
