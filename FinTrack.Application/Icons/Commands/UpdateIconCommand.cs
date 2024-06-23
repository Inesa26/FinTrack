using FinTrack.Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.Application.Icons.Commands
{
    public class UpdateIconCommand : IRequest<IconDto>
    {
        public UpdateIconCommand(int iconId, string filePath)
        {
            IconId = iconId;
            FilePath = filePath;
        }

        [Required(ErrorMessage = "Icon ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "IconId must be greater than 0")]
        public int IconId { get; set; }
        [Required(ErrorMessage = "File path is required")]
        public string FilePath { get; set; }
    }

}
