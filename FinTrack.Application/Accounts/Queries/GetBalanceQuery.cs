using FinTrack.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Application.Accounts.Queries
{
    public class GetBalanceQuery : IRequest<AccountDto>
    {
        public int AccountId { get; set; }
    }
}
