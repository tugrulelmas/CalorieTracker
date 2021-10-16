using CalorieTracker.Models.FoodEntries;
using CalorieTracker.Models.Roles;
using System;
using System.Collections.Generic;

namespace CalorieTracker.Models.Users {
    public class User {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int MaximumCalorie { get; set; }

        public ICollection<Role> Roles { get; set; }

        public ICollection<FoodEntry> FoodEntries { get; set; }
    }
}
