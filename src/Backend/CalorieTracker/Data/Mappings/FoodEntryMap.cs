using CalorieTracker.Models.FoodEntries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalorieTracker.Data.Mappings {
    class FoodEntryMap : IEntityTypeConfiguration<FoodEntry> {
        private const int FoodNameLength = 200;

        public void Configure(EntityTypeBuilder<FoodEntry> builder) {
            builder.ToTable("food_entry", "ct");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id").IsRequired();
            builder.Property(x => x.FoodName).HasColumnName("food_name").HasMaxLength(FoodNameLength);
            builder.Property(x => x.Calorie).HasColumnName("calorie");
            builder.Property(x => x.Date).HasColumnName("date");
        }
    }
}
