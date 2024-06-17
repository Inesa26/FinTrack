using FinTrack.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Application.Responses
{
    public class CategoryIconDto
    {
            public int Id { get; set; }
            public string Title { get; set; }
            public IconDto Icon { get; set; }
           
    }
}
