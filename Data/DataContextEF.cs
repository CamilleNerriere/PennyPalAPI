using PennyPal.Models;
using Microsoft.EntityFrameworkCore;
using PennyPal.Data.Configurations;

namespace PennyPal.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        public virtual DbSet<Auth> Auth {get; set;}
        public virtual DbSet<User> User {get; set;}
        public virtual DbSet<ExpenseCategory> ExpenseCategory {get; set;}
        public virtual DbSet<Expense> Expense {get; set;}
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                .UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Appliquer toutes les configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AuthConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}