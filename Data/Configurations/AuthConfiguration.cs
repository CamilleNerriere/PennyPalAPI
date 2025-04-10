using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PennyPal.Models;

namespace PennyPal.Data.Configurations
{
    public class AuthConfiguration : IEntityTypeConfiguration<Auth>
    {
        public void Configure(EntityTypeBuilder<Auth> builder)
        {
            builder.ToTable("auth")
            .HasKey(u => u.Email);

            builder.Property(e => e.Email).HasColumnName("email").IsRequired();
            builder.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
            builder.Property(e => e.PasswordSalt).HasColumnName("password_salt").IsRequired();
        
            builder.HasOne(e => e.User)
                .WithOne(e => e.Auth)
                .HasForeignKey<Auth>(e => e.Email)
                .HasPrincipalKey<User>(u => u.Email)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
