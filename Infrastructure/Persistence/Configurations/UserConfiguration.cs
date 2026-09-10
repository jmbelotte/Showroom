using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(user => user.id);

            builder.Property(user => user.id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(user => user.first_name)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(user => user.last_name)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(user => user.email)
                .HasColumnName("email")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(user => user.date_created)
                .HasColumnName("date_created")
                .IsRequired();

            builder.HasIndex(user => user.email)
                .IsUnique();
        }
    }
}
