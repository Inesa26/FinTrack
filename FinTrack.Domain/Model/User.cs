using System.ComponentModel.DataAnnotations;

namespace FinTrack.Domain.Model
{
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual Account? Account { get; set; }
        public User(string firstName, string lastName, string email, string password) 
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }
    }
}
