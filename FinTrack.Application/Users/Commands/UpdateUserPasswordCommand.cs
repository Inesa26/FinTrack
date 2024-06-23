using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Users.Commands
{
    public class UpdateUserPasswordCommand : IRequest<bool>
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(10, ErrorMessage = "Password must be at least 10 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d\s]).{10,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public string NewPassword { get; set; }
    }
}
