using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Users.Commands
{
    public class UpdateUserPasswordCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
