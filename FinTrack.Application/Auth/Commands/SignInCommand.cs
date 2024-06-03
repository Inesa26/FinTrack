using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Auth.Commands
{
    public class SignInCommand : IRequest<string>
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
