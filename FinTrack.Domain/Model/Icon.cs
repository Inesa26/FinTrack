using FinTrack.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Domain.Model
{
    public class Icon : Entity
    {
        public byte[] Data { get; set; } 
        public TransactionType TransactionType { get; set; }
        public virtual Category? Category { get; set; }
        public Icon(byte[] data, TransactionType transactionType) {
            Data = data;
            TransactionType = transactionType;
        }
    }
}
