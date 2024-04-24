using FinTrack.Application.Abstractions;
using FinTrack.Application.Categories.Commands;
using FinTrack.Application.Categories.Queries;
using FinTrack.Application.Icons.Commands;
using FinTrack.Domain.Enum;
using FinTrack.Domain.Model;
using FinTrack.Infrastructure;
using FinTrack.Infrastructure.Data;
using FinTrack.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

var mediator = Init();
static IMediator Init()
{
    var diContainer = new ServiceCollection()
        .AddDbContext<FinTrackDbContext>()
        .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IRepository<Category>).Assembly))
        .AddScoped<IUnitOfWork, UnitOfWork>()
        .AddScoped<IRepository<Category>, RepositoryImpl<Category>>()
        .AddScoped<IRepository<Icon>, RepositoryImpl<Icon>>()
        .BuildServiceProvider();

    return diContainer.GetRequiredService<IMediator>();
}

//Adding new icons
try
{
    var icon1 = await mediator.Send(new CreateIcon("D:\\Amdaris_Internship\\FinTrack\\Icons\\icon1.png"));
    var icon2 = await mediator.Send(new CreateIcon("D:\\Amdaris_Internship\\FinTrack\\Icons\\icon2.png"));
    var icon3 = await mediator.Send(new CreateIcon("D:\\Amdaris_Internship\\FinTrack\\Icons\\icon3.png"));
}
catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}

//Updating an existing Icon
try
{
    var icon1 = await mediator.Send(new UpdateIcon(4, "D:\\Amdaris_Internship\\FinTrack\\Icons\\icon2.png"));
}
catch (FileNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}

//Removing an existing icon
try
{
    var icon1 = await mediator.Send(new DeleteIcon(4));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}

// Adding a new category
try
{
    var category1 = await mediator.Send(new CreateCategory("Transport", 3, TransactionType.Expense));
    Console.WriteLine(category1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}

//Update a category
try
{
    var category1 = await mediator.Send(new UpdateCategory(1, "Transport", 1003, TransactionType.Expense));
    Console.WriteLine(category1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}

//Delete existing category
try
{
    var category1 = await mediator.Send(new DeleteCategory(3));
    Console.WriteLine(category1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}

//Get all categories
var categories = await mediator.Send(new GetAllCategories());
categories.ForEach(category => Console.WriteLine(category));

//Get category by id
Console.WriteLine(await mediator.Send(new GetCategoryById(6)));