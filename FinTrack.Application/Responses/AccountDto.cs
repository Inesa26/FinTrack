using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Application.Responses
{
    public class AccountDto
    {
        public decimal Balance { get; set; } 
        public string UserId { get; set; }
    }
}
