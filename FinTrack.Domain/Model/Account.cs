namespace FinTrack.Domain.Model
{
    public class Account : Entity
    {
        public decimal Balance { get; set; } = 0.00m;
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public virtual ICollection<Transaction>? Transactrions { get; set; }

        public Account(string userId)
        {
            UserId = userId;
        }
    }
}
