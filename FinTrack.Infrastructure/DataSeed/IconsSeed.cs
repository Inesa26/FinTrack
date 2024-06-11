using FinTrack.Application.Icons.Commands;
using FinTrack.Domain.Enum;
using FinTrack.Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrack.Infrastructure.DataSeed
{
    public class IconsSeed
    {
        public static async Task Seed(FinTrackDbContext context, IMediator mediator)
        {
            if (!context.Icons.Any())
            {
                var expenseIcons = new[]
                {
                    "car-rental.png",
                    "t-shirt.png",
                    "university.png",
                    "zoo.png",
                    "airport.png",
                    "amusement-park.png",
                    "art.png",
                    "atm.png",
                    "barbershop.png",
                    "beach.png",
                    "boat.png",
                    "bus-station.png",
                    "car-repair.png",
                    "car-wash.png",
                    "charging-station.png",
                    "church.png",
                    "cinema.png",
                    "coffee-shop.png",
                    "concert.png",
                    "drugstore.png",
                    "florist.png",
                    "football-field.png",
                    "forest.png",
                    "gas-station.png",
                    "gym.png",
                    "hospital.png",
                    "hotel.png",
                    "house.png",
                    "lake.png",
                    "laundry.png",
                    "library.png",
                    "make-up.png",
                    "monument.png",
                    "museum.png",
                    "night-club.png",
                    "office.png",
                    "park.png",
                    "parking-area.png",
                    "pet-shop.png",
                    "police-station.png",
                    "post-office.png",
                    "railway-station.png",
                    "restaurant.png",
                    "river.png",
                    "school.png",
                    "shop.png",
                    "sports.png",
                    "apple.png",
                    "gift.png",
                    "hand-bag.png",
                    "eyeglasses.png",
                    "shoes.png",
                    "ring.png",
                    "fast-food.png",
                    "dining-table.png",
                };
                var incomeIcons = new[]
              {
                    "puzzle.png",
                    "briefcase.png",
                    "negotiation.png",
                    "presentation.png",
                    "profit.png",
                    "credit-card.png",
                    "cash.png",
                };

                foreach (var iconName in expenseIcons)
                {
                    var iconPath = Path.Combine("D:\\Amdaris_Internship\\FinTrack\\Icons", iconName);
                    TransactionType transactionType = TransactionType.Expense;

                    var command = new CreateIconCommand(iconPath, transactionType);
                    await mediator.Send(command);
                }

                foreach (var iconName in incomeIcons)
                {
                    var iconPath = Path.Combine("D:\\Amdaris_Internship\\FinTrack\\Icons", iconName);
                    TransactionType transactionType = TransactionType.Income;

                    var command = new CreateIconCommand(iconPath, transactionType);
                    await mediator.Send(command);
                }
            }
        }
    }
}