using CalorieTracker.Models.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalorieTracker.Data.Mappings {
    class RoleMap : IEntityTypeConfiguration<Role> {
        private const int NameLength = 200;

        public void Configure(EntityTypeBuilder<Role> builder) {
            builder.ToTable("role", "ct");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id").IsRequired();
            builder.Property(x => x.Name).HasColumnName("name").IsRequired().HasMaxLength(NameLength);
        }
    }
}
