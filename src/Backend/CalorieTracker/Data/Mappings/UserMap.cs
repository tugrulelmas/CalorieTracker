using CalorieTracker.Models.Roles;
using CalorieTracker.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace CalorieTracker.Data.Mappings {
    class UserMap : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.ToTable("user", "ct");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id").IsRequired();
            builder.Property(x => x.Name).HasColumnName("name").IsRequired();
            builder.Property(x => x.Email).HasColumnName("email").IsRequired();
            builder.Property(x => x.MaximumCalorie).HasColumnName("maximum_calorie").IsRequired();
            builder.HasMany(x => x.Roles).WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "ct.user_role",
                    j => j
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("role_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.ClientCascade));

            builder.HasMany(x => x.FoodEntries).WithOne(x => x.User).HasForeignKey("user_id");
        }
    }
}
