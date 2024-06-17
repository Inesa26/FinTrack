using FinTrack.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Application.Responses
{
    public class TransactionCategoryDto
    {
        public int Id { get; init; }
        public int AccountId { get; init; }
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public string Description { get; set; }
        public CategoryIconDto Category { get; set; }
        public TransactionType TransactionType { get; set; }

    }
}
