using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PennyPal.Models;

namespace PennyPal.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens")
            .HasKey(rt => rt.Id);

            builder.Property(rt => rt.Id).HasColumnName("id");
            builder.Property(rt => rt.Token).HasColumnName("token");
            builder.Property(rt => rt.Expires).HasColumnName("expires");
            builder.Property(rt => rt.CreatedAt).HasColumnName("created_at");
            builder.Property(rt => rt.CreatedByIp).HasColumnName("created_by_ip");
            builder.Property(rt => rt.Revoked).HasColumnName("revoked");
            builder.Property(rt => rt.ReplacedByToken).HasColumnName("replaced_by_token");
            builder.Property(rt => rt.SessionExpiresAt).HasColumnName("session_expires_at");
            builder.Property(rt => rt.UserId).HasColumnName("user_id");

            builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
             .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}