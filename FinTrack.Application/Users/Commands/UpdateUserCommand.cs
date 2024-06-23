using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Users.Commands
{
    public class UpdateUserCommand : IRequest<UpdatedUserDto>
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(50, ErrorMessage = "Email address cannot exceed 50 characters")]
        public string Email { get; set; }

    }
}
