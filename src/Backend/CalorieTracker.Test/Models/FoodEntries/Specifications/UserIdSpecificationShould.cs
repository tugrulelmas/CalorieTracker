using CalorieTracker.Models.FoodEntries;
using CalorieTracker.Models.FoodEntries.Specifications;
using CalorieTracker.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CalorieTracker.Test.Models.FoodEntries.Specifications {
    public class UserIdSpecificationShould {
        [Fact]
        public void FilterFoodEntriesWhenTheIdIsNotEmpty() {
            var userId = Guid.NewGuid();
            var userIdSpecification = new UserIdSpecification(userId);

            var foodEntries = new List<FoodEntry> {
                        new FoodEntry { User = new User { Id = Guid.NewGuid() }},
                        new FoodEntry { User = new User { Id = userId }}
            };

            var filteredFoodEntries = foodEntries.Where(userIdSpecification.ToExpression().Compile());

            Assert.Single(filteredFoodEntries);
            Assert.Equal(userId, filteredFoodEntries.First().User.Id);
        }

        [Fact]
        public void NotFilterFoodEntriesWhenTheIdIsEmpty() {
            var userIdSpecification = new UserIdSpecification(Guid.Empty);

            var foodEntries = new List<FoodEntry> {
                        new FoodEntry { User = new User { Id = Guid.NewGuid() }},
                        new FoodEntry { User = new User { Id = Guid.NewGuid() }}
            };

            var filteredFoodEntries = foodEntries.Where(userIdSpecification.ToExpression().Compile());

            Assert.Equal(foodEntries.Count, filteredFoodEntries.Count());
        }
    }
}
