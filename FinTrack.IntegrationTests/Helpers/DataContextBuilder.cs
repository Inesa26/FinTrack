using FinTrack.Domain.Enum;
using FinTrack.Domain.Model;
using FinTrack.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinTrack.IntegrationTests.Helpers
{
    public class DataContextBuilder : IDisposable
    {
        private readonly FinTrackDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public DataContextBuilder(string dbName = null)
        {
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            if (string.IsNullOrEmpty(dbName))
            {
                dbName = Guid.NewGuid().ToString();
            }

            services.AddDbContext<FinTrackDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: dbName);
                options.ConfigureWarnings(warnings =>
                {
                    warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                });
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<FinTrackDbContext>()
                .AddDefaultTokenProviders();

            var serviceProvider = services.BuildServiceProvider();

            _dbContext = serviceProvider.GetRequiredService<FinTrackDbContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        }

        public FinTrackDbContext GetContext()
        {
            _dbContext.Database.EnsureCreated();
            return _dbContext;
        }

        public async Task SeedUsersAsync(int number = 1)
        {
            for (var i = 0; i < number; i++)
            {
                var user = new ApplicationUser
                {
                    UserName = $"user{i}@gmail.com",
                    Email = $"user{i}@gmail.com",
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}"
                };


                var result = await _userManager.CreateAsync(user, "Password123@");
                if (!result.Succeeded)
                {
                    throw new Exception("User seeding failed.");
                }
            }
        }

        public async Task SeedAccountsAsync(int number = 1)
        {
            var users = await _userManager.Users.Take(number).ToListAsync();

            if (!users.Any())
            {
                throw new InvalidOperationException("No users found to seed accounts. Please seed users first.");
            }

            var accounts = users.Select(user => new Account(user.Id)).ToList();

            _dbContext.AddRange(accounts);
            await _dbContext.SaveChangesAsync();
        }

        public void SeedIcons(int number = 1)
        {
            var icons = new List<Icon>();

            for (var i = 0; i < number; i++)
            {
                var data = new byte[] { 0xFF, 0xAA, 0x55, 0x00 };
                TransactionType transactionType = TransactionType.Expense;
                var icon = new Icon(data, transactionType);

                icons.Add(icon);
            }

            _dbContext.AddRange(icons);
            _dbContext.SaveChanges();
        }

        public void SeedCategories(int number = 1)
        {
            if (!_dbContext.Icons.Any())
            {
                SeedIcons(number);
            }

            var iconIds = _dbContext.Icons.Select(i => i.Id).ToList();

            if (number > iconIds.Count)
            {
                throw new InvalidOperationException("Not enough icons to seed the requested number of categories.");
            }

            var categories = new List<Category>();

            for (var i = 0; i < number; i++)
            {
                string title = $"Category {i + 1}";
                TransactionType type = TransactionType.Expense;
                int iconId = iconIds[i];

                var category = new Category(title, type, iconId);

                categories.Add(category);
            }

            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();
        }

        public async Task SeedTransactionsAsync(int transactionsPerAccount = 1)
        {
            var accounts = await _dbContext.Accounts.ToListAsync();
            var categories = await _dbContext.Categories.Take(transactionsPerAccount).ToListAsync();

            if (!accounts.Any())
            {
                throw new InvalidOperationException("No accounts found to seed transactions. Please seed accounts first.");
            }

            if (!categories.Any())
            {
                throw new InvalidOperationException("No categories found to seed transactions. Please seed categories first.");
            }

            var transactions = new List<Transaction>();

            foreach (var account in accounts)
            {
                for (var i = 0; i < transactionsPerAccount; i++)
                {
                    var categoryId = categories[0].Id;
                    var amount = (i + 1) * 10.00m;
                    var date = DateTime.Now.AddDays(-i);
                    var description = $"Transaction {i + 1} for Account {account.Id}";

                    var transaction = new Transaction(account.Id, amount, date, description, categoryId);

                    transactions.Add(transaction);
                }
            }

            _dbContext.AddRange(transactions);
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Database.EnsureDeleted();
                _dbContext.Dispose();
            }
        }
    }
}
