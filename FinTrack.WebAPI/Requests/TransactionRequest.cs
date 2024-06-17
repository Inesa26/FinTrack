using FinTrack.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.WebAPI.Requests
{
    public class TransactionRequest
    {
        public TransactionRequest(decimal amount, DateTime date, string description, int categoryId, TransactionType transactionType)
        {
            Amount = amount;
            Date = date;
            Description = description;
            CategoryId = categoryId;
            TransactionType = transactionType;
        }

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

        [Required(ErrorMessage = "Transaction is required")]
        public TransactionType TransactionType { get; set; }
    }
}
