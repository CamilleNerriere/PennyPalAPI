using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PennyPal.Models;

namespace PennyPal.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users")
            .HasKey(u => u.Id);

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Email).HasColumnName("email").IsRequired();
            builder.HasAlternateKey(e => e.Email);
            builder.Property(e => e.Lastname).HasColumnName("lastname").IsRequired();
            builder.Property(e => e.Firstname).HasColumnName("firstname").IsRequired();


            builder.HasMany(e => e.ExpenseCategories)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Expenses)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);

        }
    }
}