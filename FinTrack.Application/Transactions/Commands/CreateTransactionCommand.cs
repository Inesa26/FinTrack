﻿using FinTrack.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Application.Transactions.Commands
{
    public class CreateTransactionCommand: IRequest<TransactionDto>
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
