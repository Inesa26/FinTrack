﻿using FinTrack.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Data
{
    public class FinTrackDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Account> Accounts { get; set; } = default!;
        public DbSet<Icon> Icons { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Transaction> Transactions { get; set; } = default!;

        public FinTrackDbContext(DbContextOptions options) : base(options)
        {
        }

        public FinTrackDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=FinanceTracker;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                 .Property(x => x.FirstName)
                 .IsRequired()
                 .HasMaxLength(20);

            modelBuilder.Entity<User>()
                 .Property(x => x.LastName)
                 .IsRequired()
                 .HasMaxLength(20);

            modelBuilder.Entity<User>()
                 .Property(x => x.Email)
                 .IsRequired()
                 .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                  .Property(x => x.Password)
                  .IsRequired()
                  .HasMaxLength(50);

            modelBuilder.Entity<Account>()
                 .Property(x => x.Balance)
                 .HasColumnType("Money")
                 .IsRequired()
                 .HasDefaultValue(0);

            modelBuilder.Entity<Account>()
                 .HasIndex(x => x.UserId)
                 .IsUnique();

            modelBuilder.Entity<Account>()
                  .Property(c => c.UserId)
                  .IsRequired();

            modelBuilder.Entity<Icon>()
                 .Property(c => c.Data)
                 .IsRequired();

            modelBuilder.Entity<Icon>()
                 .Property(x => x.Data)
                 .HasColumnType("VARBINARY(MAX)");

            modelBuilder.Entity<Category>()
                 .Property(c => c.Title)
                 .HasMaxLength(20)
                 .IsRequired();

            modelBuilder.Entity<Category>()
                 .HasIndex(x => x.Title)
                 .IsUnique();

            modelBuilder.Entity<Category>()
                 .Property(c => c.Type)
                 .IsRequired()
                 .HasColumnType("NVARCHAR(20)");

            modelBuilder.Entity<Category>()
                 .Property(c => c.IconId)
                 .IsRequired();

            modelBuilder.Entity<Category>()
                .HasIndex(x => x.IconId)
                .IsUnique(false);

            modelBuilder.Entity<Transaction>()
               .Property(x => x.Amount)
               .IsRequired()
               .HasColumnType("Money");

            modelBuilder.Entity<Transaction>()
                .Property(c => c.Date)
                .IsRequired();

            modelBuilder.Entity<Transaction>()
                .Property(c => c.CategoryId)
                .IsRequired();

            modelBuilder.Entity<Transaction>()
                .Property(c => c.AccountId)
                .IsRequired();

            modelBuilder.Entity<Transaction>()
               .Property(x => x.Description)
               .HasMaxLength(100);
        }
    }
}