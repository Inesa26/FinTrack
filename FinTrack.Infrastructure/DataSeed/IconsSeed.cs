using FinTrack.Application.Icons.Commands;
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
                var iconsToSeed = new[]
                {
                new CreateIconCommand("D:\\Amdaris_Internship\\FinTrack\\Icons\\icon1.png"),
                new CreateIconCommand("D:\\Amdaris_Internship\\FinTrack\\Icons\\icon2.png"),
                new CreateIconCommand("D:\\Amdaris_Internship\\FinTrack\\Icons\\icon3.png"),
            };

                foreach (var iconCommand in iconsToSeed)
                {
                    await mediator.Send(iconCommand);
                }
            }
        }
    }
}