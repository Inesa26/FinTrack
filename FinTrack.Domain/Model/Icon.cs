using FinTrack.Domain.Enum;

namespace FinTrack.Domain.Model
{
    public class Icon : Entity
    {
        public byte[] Data { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Title { get; set; }
        public virtual Category? Category { get; set; }
        public Icon(byte[] data, TransactionType transactionType, string title)
        {
            Data = data;
            TransactionType = transactionType;
            Title = title;
        }
    }
}
