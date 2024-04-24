using System.Collections.Generic;

namespace FinTrack.Domain.Model
{
    public class Account : Entity
    {
        
        public decimal Balance { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public virtual ICollection<Transaction>? Transactrions { get; set; }

        public Account(int userId, decimal balance) 
        {
            UserId = userId;
            Balance = balance;
        }
    }
}
