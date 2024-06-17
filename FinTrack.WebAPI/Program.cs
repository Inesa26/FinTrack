using FinTrack.Application.Abstractions;
using FinTrack.Domain.Model;
using FinTrack.Infrastructure;
using FinTrack.Infrastructure.Data;
using FinTrack.Infrastructure.DataSeed;
using FinTrack.Infrastructure.Repositories;
using FinTrack.Infrastructure.Services;
using FinTrack.WebAPI.Extensions;
using FinTrack.WebAPI.Middleware;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

builder.Services
    .AddSingleton<FinTrackDbContext>()
    .AddSingleton<IRepository<Category>, RepositoryImpl<Category>>()
    .AddSingleton<IRepository<Icon>, RepositoryImpl<Icon>>()
    .AddSingleton<IRepository<Transaction>, RepositoryImpl<Transaction>>()
    .AddSingleton<IRepository<Account>, RepositoryImpl<Account>>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<IIdentityService, IdentityService>()
    .AddScoped<IUserAuthenticationService, UserAuthenticationService>()
    .AddScoped<ITokenService, TokenService>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IRepository<Category>).Assembly));

builder.Services.AddScoped<UserManager<ApplicationUser>>();

// Add ASP.NET Core Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<FinTrackDbContext>()
    .AddDefaultTokenProviders();

builder.RegisterAuthentication();

// Register AutoMapper with the dependency injection container
builder.Services.AddAutoMapper(typeof(IRepository<Category>).Assembly);

// Add CORS services to the container
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:5173") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwagger();

var app = builder.Build();

// Seed the database during application startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<FinTrackDbContext>();
    var mediator = services.GetRequiredService<IMediator>();

    await IconsSeed.SeedIcons(context, mediator);
    await CategoriesSeed.SeedCategories(context, mediator);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCustomExceptionHandler();

app.UseTimingLogger();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

// Use CORS middleware
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
