using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PennyPal.Models;

namespace PennyPal.Data.Configurations
{
    public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            builder.ToTable("expense_categories")
            .HasKey(u => u.Id);

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            builder.Property(e => e.MonthlyBudget).HasColumnName("monthly_budget").IsRequired();

            builder.HasMany(e => e.Expenses)
                .WithOne(e => e.ExpenseCategory)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}