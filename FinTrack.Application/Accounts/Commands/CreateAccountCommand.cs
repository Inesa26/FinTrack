using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Accounts.Commands
{
    public class CreateAccountCommand : IRequest<AccountDto>
    {
        public CreateAccountCommand(string id)
        {
            Id = id;
        }

        [Required]
        public string Id { get; set; }
    }
}
