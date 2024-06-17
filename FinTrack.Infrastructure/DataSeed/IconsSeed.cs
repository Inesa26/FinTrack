using FinTrack.Application.Icons.Commands;
using FinTrack.Domain.Enum;
using FinTrack.Infrastructure.Data;
using MediatR;

namespace FinTrack.Infrastructure.DataSeed
{
    public class IconsSeed
    {
        public static async Task SeedIcons(FinTrackDbContext context, IMediator mediator)
        {
            if (!context.Icons.Any())
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
                    "presents.png",
                    "quick-eats.png",
                    "recreation.png",
                    "religion.png",
                    "restaurant.png",
                    "school.png",
                    "sports.png",
                    "transport.png",
                    "travel.png",
                    "zoo.png",
                    "phone.png",
                    "love.png",
                    "wedding-rings.png",
                    "arcade-game.png",
                    "card-game.png",
                    "recycle-bin.png",
                    "mop.png",
                    "hammer.png",
                    "paper-clip.png",
                    "cake.png",
                    "cocktail.png",
                    "photography.png",
                    "hunting.png",
                    "computer.png",
                    "balloons.png",
                    "cat.png",
                    "dog.png",
                    "family.png",
                    "boy.png",
                    "girl.png",
                    "trains.png"
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

                // Seed expense icons
                foreach (var iconName in expenseIcons)
                {
                    var iconPath = Path.Combine("D:\\Amdaris_Internship\\FinTrack\\Icons", iconName);
                    TransactionType transactionType = TransactionType.Expense;


                    var createIconCommand = new CreateIconCommand(iconPath, transactionType, iconName);
                    var icon = await mediator.Send(createIconCommand);
                }

                // Seed income icons
                foreach (var iconName in incomeIcons)
                {
                    var iconPath = Path.Combine("D:\\Amdaris_Internship\\FinTrack\\Icons", iconName);
                    TransactionType transactionType = TransactionType.Income;

                    var createIconCommand = new CreateIconCommand(iconPath, transactionType, iconName);
                    var icon = await mediator.Send(createIconCommand);
                }
            }
        }
    }
}
