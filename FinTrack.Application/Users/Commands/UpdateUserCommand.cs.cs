using MediatR;
using FinTrack.Application.Responses;

namespace FinTrack.Application.Users.Commands
{
    public class UpdateUserCommand : IRequest<UpdatedUserDto>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
       
    }
}
