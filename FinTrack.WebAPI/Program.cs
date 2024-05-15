using FinTrack.Application.Abstractions;
using FinTrack.Domain.Model;
using FinTrack.Infrastructure;
using FinTrack.Infrastructure.Data;
using FinTrack.Infrastructure.Repositories;
using FinTrack.Infrastructure.Services;
using FinTrack.WebAPI.Extensions;
using FinTrack.WebAPI.Middleware;
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
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<IIdentityService, IdentityService>()
    .AddScoped<IUserAuthenticationService, UserAuthenticationService>()
    .AddScoped<ITokenService, TokenService>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IRepository<Category>).Assembly));

builder.Services.AddScoped<UserManager<ApplicationUser>>();

//Add ASP.NET Core Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<FinTrackDbContext>()
    .AddDefaultTokenProviders();

builder.RegisterAuthentication();

// Register AutoMapper with the dependency injection container
builder.Services.AddAutoMapper(typeof(IRepository<Category>).Assembly);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwagger();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
