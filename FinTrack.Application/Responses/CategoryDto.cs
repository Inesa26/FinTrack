using FinTrack.Domain.Model;
using System.Xml.Linq;

namespace FinTrack.Application.Responses
{
    public class CategoryDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public int IconId { get; init; }

        public static CategoryDto FromCategory(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Title = category.Title,
                IconId = category.IconId
            };
        }

        public override string? ToString()
        {
            return $"Id: {Id}, Title: {Title ?? "N/A"}";
        }
    }
}
