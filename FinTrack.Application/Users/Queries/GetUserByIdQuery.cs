using FinTrack.Application.Responses;
using MediatR;

namespace FinTrack.Application.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public GetUserByIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }
}
