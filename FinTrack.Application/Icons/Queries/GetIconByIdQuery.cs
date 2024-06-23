using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Icons.Queries
{
    public class GetIconByIdQuery : IRequest<IconDto>
    {
        public GetIconByIdQuery(int iconId)
        {
            IconId = iconId;
        }
        [Required(ErrorMessage = "Icon Id is required")]
        public int IconId { get; set; }
    }
}
