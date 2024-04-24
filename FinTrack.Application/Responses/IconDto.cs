using FinTrack.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Application.Responses
{
   public class IconDto
    {
        public int Id { get; init; }
        byte[] Data { get; init; }

        public static IconDto FromCategory(Icon icon)
        {
            return new IconDto
            {
                Id = icon.Id,
                Data = icon.Data,    
            };
        }
    }
}
