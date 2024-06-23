using FinTrack.Domain.Enum;

namespace FinTrack.Application.Responses
{
    public class IconDto
    {
        public int Id { get; init; }
        public string Base64Data { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Title { get; set; }
    }
}
