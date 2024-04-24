using FinTrack.Domain.Enum;
using System.Collections.Generic;

namespace FinTrack.Domain.Model
{
    public class Category : Entity
    {
        public string Title { get; set; }
        public TransactionType Type { get; set; }
        public int IconId { get; set; }
        public virtual Icon? Icon { get; set; }
        public virtual ICollection<Transaction>? transactions { get; set; }   

        public Category(string title, TransactionType type, int iconId) {
            Title = title;
            Type = type; 
            IconId = iconId;
        }
    }
}