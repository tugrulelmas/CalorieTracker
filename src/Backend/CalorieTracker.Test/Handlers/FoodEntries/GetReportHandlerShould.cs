using CalorieTracker.Data;
using CalorieTracker.Handlers.FoodEntries;
using CalorieTracker.Models.FoodEntries;
using CalorieTracker.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CalorieTracker.Test.Handlers.FoodEntries {
    public class GetReportHandlerShould {

        [Fact]
        public async Task ReturnEmptyWhenTheUserDoesntHaveAnyFoodEntryInsideTheGivingDates() {
            var options = new DbContextOptionsBuilder<CalorieTrackerContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var userId = Guid.NewGuid();
            var user = new User {
                Id = userId,
                Email = "test@email.com",
                Name = "user name"
            };

            var foodEntry = new FoodEntry {
                Calorie = 12,
                FoodName = "food name",
                Date = DateTime.Today.AddDays(-8),
                User = user
            };

            using (var context = new CalorieTrackerContext(options)) {
                await context.Users.AddAsync(user);
                await context.FoodEntries.AddAsync(foodEntry);

                await context.SaveChangesAsync();
            }

            using (var context = new CalorieTrackerContext(options)) {
                var getReportHandler = new GetReportHandler(context);
                var getReportRequest = new GetReportRequest(1, 10, DateTime.Today);

                var result = await getReportHandler.Handle(getReportRequest, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(0, result.AddedCount);
                Assert.Equal(0, result.TotalCount);
                Assert.Empty(result.AverageCalories);
            }
        }

        [Fact]
        public async Task ReturnAverageCalories() {
            var options = new DbContextOptionsBuilder<CalorieTrackerContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var userId = Guid.NewGuid();
            var user = new User {
                Id = userId,
                Email = "test@email.com",
                Name = "user name"
            };

            var user2 = new User {
                Email = "test@email.com",
                Name = "user name"
            };

            var foodEntries = new List<FoodEntry>();

            foodEntries.Add(new FoodEntry {
                Calorie = 12,
                FoodName = "food name",
                Date = DateTime.Today,
                User = user2
            });

            foodEntries.Add(new FoodEntry {
                Calorie = 12,
                FoodName = "food name",
                Date = DateTime.Today.AddDays(-12),
                User = user
            });

            foodEntries.AddRange(Enumerable.Range(0, 8).Select(x =>
                  new FoodEntry {
                      Calorie = 12,
                      FoodName = "food name",
                      Date = DateTime.Today.AddDays(x * -1),
                      User = user
                  }));

            using (var context = new CalorieTrackerContext(options)) {
                await context.Users.AddAsync(user);
                await context.FoodEntries.AddRangeAsync(foodEntries);

                await context.SaveChangesAsync();
            }

            using (var context = new CalorieTrackerContext(options)) {
                var getReportHandler = new GetReportHandler(context);
                var getReportRequest = new GetReportRequest(1, 10, DateTime.Today);

                var result = await getReportHandler.Handle(getReportRequest, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(9, result.AddedCount);
                Assert.Equal(2, result.TotalCount);
                Assert.NotEmpty(result.AverageCalories);
                Assert.Equal(2, result.AverageCalories.Count());
            }
        }
    }
}
