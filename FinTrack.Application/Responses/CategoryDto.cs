using FinTrack.Domain.Enum;

namespace FinTrack.Application.Responses
{
    public class CategoryDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public int IconId { get; init; }
        public TransactionType Type { get; set; }

        public override string? ToString()
        {
            return $"Id: {Id}, Title: {Title ?? "N/A"}";
        }
    }
}
