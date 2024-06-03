using AutoMapper;
using FinTrack.Application.Abstractions;
using FinTrack.Domain.Model;
using FinTrack.Infrastructure;
using FinTrack.Infrastructure.Data;
using FinTrack.Infrastructure.Options;
using FinTrack.Infrastructure.Repositories;
using FinTrack.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FinTrack.IntegrationTests.Helpers
{
    public class TestHelpers
    {
        public static IMapper CreateMapper()
        {
            var services = new ServiceCollection();

            services.AddAutoMapper(typeof(IRepository<Category>).Assembly);

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMapper>();
        }

        public static IUnitOfWork CreateUnitOfWork(FinTrackDbContext dbContext)
        {
            var categoryRepository = new RepositoryImpl<Category>(dbContext);
            var iconRepository = new RepositoryImpl<Icon>(dbContext);
            var transactionRepository = new RepositoryImpl<Transaction>(dbContext);

            // UserManager configuration
            var userStore = new UserStore<ApplicationUser>(dbContext);
            var passwordHasher = new PasswordHasher<ApplicationUser>(); 
            var userManager = new UserManager<ApplicationUser>(userStore, null, passwordHasher, null, null, null, null, null, null);

            var roleStore = new RoleStore<IdentityRole>(dbContext);
            var roleManager = new RoleManager<IdentityRole>(roleStore, null, null, null, null);

            // UserRepository creation
            var userRepository = new UserRepository(userManager, roleManager);

            var accountRepository = new RepositoryImpl<Account>(dbContext);

            return new UnitOfWork(dbContext, categoryRepository, iconRepository, transactionRepository, userRepository, accountRepository);
        }
      

        public static ILogger<T> CreateLogger<T>()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            return loggerFactory.CreateLogger<T>();
        }

        public static IOptions<JwtSettings> CreateJwtSettingsOptions()
        {
            var mockOptions = new Mock<IOptions<JwtSettings>>();
            var jwtSettings = new JwtSettings
            {
                SecretKey = "gJ!mNcRfUjXn2r5u8x/A?D(G+KbPeShVm",
                Issuer = "FinTrack",
                Audiences = new[] { "Swagger-Client" }
            };
            mockOptions.Setup(opt => opt.Value).Returns(jwtSettings);
            return mockOptions.Object;
        }
      
        public static IMediator CreateMediator(FinTrackDbContext dbContext, ILogger logger, IMapper mapper)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IRepository<Category>).Assembly));
            services.AddScoped(_ => CreateUnitOfWork(dbContext));
            services.AddSingleton(logger);
            services.AddSingleton(mapper);

            // Add the mock for JwtSettings
            services.AddSingleton(CreateJwtSettingsOptions());

            // Add other services
            services.AddSingleton(dbContext);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<FinTrackDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<UserManager<ApplicationUser>>();
           
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMediator>();
        }

    }
}
