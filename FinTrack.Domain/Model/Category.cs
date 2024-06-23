using FinTrack.Domain.Enum;

namespace FinTrack.Domain.Model
{
    public class Category : Entity
    {
        public string Title { get; set; }
        public TransactionType Type { get; set; }
        public int IconId { get; set; }
        public int AccoundId { get; set; }
        public virtual Icon? Icon { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }

        public Category(string title, TransactionType type, int iconId)
        {
            Title = title;
            Type = type;
            IconId = iconId;
        }
    }
}