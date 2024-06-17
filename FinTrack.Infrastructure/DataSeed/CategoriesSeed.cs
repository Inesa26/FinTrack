using FinTrack.Application.Categories.Commands;
using FinTrack.Domain.Enum;
using FinTrack.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.DataSeed
{
    public class CategoriesSeed
    {
        public static async Task SeedCategories(FinTrackDbContext context, IMediator mediator)
        {
            if (!context.Categories.Any())
            {
                var expenseIcons = new[]
            {
                    "accomodation.png",
                    "art.png",
                    "bills.png",
                    "books.png",
                    "car-rental.png",
                    "car-repair.png",
                    "car-wash.png",
                    "charging-station.png",
                    "cinema.png",
                    "clothes.png",
                    "concert.png",
                    "cosmetics.png",
                    "donations.png",
                    "education.png",
                    "financial.png",
                    "fines.png",
                    "fitness.png",
                    "flowers.png",
                    "fuel.png",
                    "groceries.png",
                    "healthcare.png",
                    "home.png",
                    "jewerly.png",
                    "laundry.png",
                    "medicines.png",
                    "nightlife.png",
                    "parking.png",
                    "personal-care.png",
                    "pets.png",
                    "phone.png",
                    "presents.png",
                    "quick-eats.png",
                    "recreation.png",
                    "religion.png",
                    "restaurant.png",
                    "school.png",
                    "sports.png",
                    "transport.png",
                    "travel.png",
                    "zoo.png"
            };

                var incomeIcons = new[]
                {
                   "parttime.png",
                    "other.png",
                    "salary.png",
                    "fund.png",
                    "stock.png",
                    "invest.png",
                    "rent.png",
                    "borrow.png"
            };

                // Seed categories for expense icons
                foreach (var iconName in expenseIcons)
                {
                    var categoryName = Path.GetFileNameWithoutExtension(iconName);
                    var icon = await context.Icons.FirstOrDefaultAsync(i => i.Title == iconName);
                    if (icon != null)
                    {
                        var createCategoryCommand = new CreateCategoryCommand(categoryName, icon.Id, TransactionType.Expense);
                        await mediator.Send(createCategoryCommand);
                    }
                }

                // Seed categories for income icons
                foreach (var iconName in incomeIcons)
                {
                    var categoryName = Path.GetFileNameWithoutExtension(iconName); // Example: "puzzle"
                    var icon = await context.Icons.FirstOrDefaultAsync(i => i.Title == iconName);
                    if (icon != null)
                    {
                        var createCategoryCommand = new CreateCategoryCommand(categoryName, icon.Id, TransactionType.Income);
                        await mediator.Send(createCategoryCommand);
                    }
                }
            }
        }
    }
}
