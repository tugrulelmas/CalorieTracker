using CalorieTracker.Models.FoodEntries;
using CalorieTracker.Models.Roles;
using CalorieTracker.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;

namespace CalorieTracker.Data {
    public class CalorieTrackerContext : DbContext {
        public CalorieTrackerContext(DbContextOptions<CalorieTrackerContext> options)
           : base(options) {
        }

        public DbSet<FoodEntry> FoodEntries { get; set; }

        public DbSet<User> Users { get; set; }
        
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            if (modelBuilder is null)
                throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
