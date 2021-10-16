using CalorieTracker.Models.FoodEntries;
using CalorieTracker.Models.FoodEntries.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CalorieTracker.Test.Models.FoodEntries.Specifications {
    public class DateGreaterThanSpecificationShould {
        [Fact]
        public void FilteFoodEntriesWhenTheDateIsNotEmpty() {
            var date = DateTime.Today;
            var dateGreaterThanSpecification = new DateGreaterThanSpecification(date);

            var foodEntries = new List<FoodEntry> {
                        new FoodEntry { Date = date },
                        new FoodEntry { Date = date.AddDays(-1) },
                        new FoodEntry { Date = date.AddDays(-7) },
                        new FoodEntry { Date = date.AddDays(2) },
                        new FoodEntry { Date = date.AddDays(8) }
            };

            var filteredFoodEntries = foodEntries.Where(dateGreaterThanSpecification.ToExpression().Compile());

            Assert.Equal(foodEntries.Count(x => x.Date >= date), filteredFoodEntries.Count());
            Assert.True(filteredFoodEntries.All(x => x.Date >= date));
        }

        [Fact]
        public void NotFilterFoodEntriesWhenTheDateIsEmpty() {
            var dateGreaterThanSpecification = new DateGreaterThanSpecification(DateTime.MinValue);

            var date = DateTime.Today;
            var foodEntries = new List<FoodEntry> {
                        new FoodEntry { Date = date },
                        new FoodEntry { Date = date.AddDays(-1) },
                        new FoodEntry { Date = date.AddDays(-7) },
                        new FoodEntry { Date = date.AddDays(2) },
                        new FoodEntry { Date = date.AddDays(8) }
            };

            var filteredFoodEntries = foodEntries.Where(dateGreaterThanSpecification.ToExpression().Compile());

            Assert.Equal(foodEntries.Count, filteredFoodEntries.Count());
        }
    }
}
