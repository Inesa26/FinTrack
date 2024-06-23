using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public GetUserByIdQuery(string userId)
        {
            UserId = userId;
        }
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }
    }
}
