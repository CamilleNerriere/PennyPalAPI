using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PennyPal.Models;

namespace PennyPal.Data.Configurations
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("expenses")
            .HasKey(u => u.Id);

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(e => e.CategoryId).HasColumnName("category_id").IsRequired();
            builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(e => e.Amount).HasColumnName("amount").IsRequired();
            builder.Property(e => e.Date)
                .HasColumnName("date")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

        }
    }
}