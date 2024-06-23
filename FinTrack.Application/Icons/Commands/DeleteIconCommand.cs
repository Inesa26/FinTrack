using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Icons.Commands
{
    public class DeleteIconCommand : IRequest<IconDto>
    {
        public DeleteIconCommand(int iconId)
        {
            IconId = iconId;
        }

        [Required(ErrorMessage = "Icon ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "IconId must be greater than 0")]
        public int IconId { get; set; }
    }

}
