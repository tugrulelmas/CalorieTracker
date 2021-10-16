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
    public class GetTotalCaloriesByDayHandlerShould {

        [Fact]
        public async Task ReturnEmptyWhenTheUserDoesntExist() {
            var options = new DbContextOptionsBuilder<CalorieTrackerContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new CalorieTrackerContext(options)) {
                var getTotalCaloriesByDayHandler = new GetTotalCaloriesByDayHandler(context);
                var getTotalCaloriesByDayRequest = new GetTotalCaloriesByDayRequest(1, 10, Guid.NewGuid(), DateTime.Today, DateTime.Today);

                var result = await getTotalCaloriesByDayHandler.Handle(getTotalCaloriesByDayRequest, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(0, result.TotalCount);
                Assert.Empty(result.Calories);
            }
        }

        [Fact]
        public async Task ReturnEmptyWhenTheUserDoesntHaveAnyFoodEntry() {
            var options = new DbContextOptionsBuilder<CalorieTrackerContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var userId = Guid.NewGuid();
            var user = new User {
                Id = userId,
                Email = "test@email.com",
                Name = "user name"
            };

            using (var context = new CalorieTrackerContext(options)) {
                await context.Users.AddAsync(user);

                await context.SaveChangesAsync();
            }

            using (var context = new CalorieTrackerContext(options)) {
                var getTotalCaloriesByDayHandler = new GetTotalCaloriesByDayHandler(context);
                var getTotalCaloriesByDayRequest = new GetTotalCaloriesByDayRequest(1, 10, userId, DateTime.Today, DateTime.Today);

                var result = await getTotalCaloriesByDayHandler.Handle(getTotalCaloriesByDayRequest, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(0, result.TotalCount);
                Assert.Empty(result.Calories);
            }
        }

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
                Date = DateTime.Today.AddDays(-1),
                User = user
            };

            using (var context = new CalorieTrackerContext(options)) {
                await context.Users.AddAsync(user);
                await context.FoodEntries.AddAsync(foodEntry);

                await context.SaveChangesAsync();
            }

            using (var context = new CalorieTrackerContext(options)) {
                var getTotalCaloriesByDayHandler = new GetTotalCaloriesByDayHandler(context);
                var getTotalCaloriesByDayRequest = new GetTotalCaloriesByDayRequest(1, 10, userId, DateTime.Today, DateTime.Today);

                var result = await getTotalCaloriesByDayHandler.Handle(getTotalCaloriesByDayRequest, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(0, result.TotalCount);
                Assert.Empty(result.Calories);
            }
        }

        [Fact]
        public async Task ReturnFoodEntries() {
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

            foodEntries.AddRange(Enumerable.Range(0, 4).Select(x =>
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
                var getTotalCaloriesByDayHandler = new GetTotalCaloriesByDayHandler(context);
                var getTotalCaloriesByDayRequest = new GetTotalCaloriesByDayRequest(1, 10, userId, DateTime.Today.AddDays(-4), DateTime.Today);

                var result = await getTotalCaloriesByDayHandler.Handle(getTotalCaloriesByDayRequest, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(4, result.TotalCount);
                Assert.NotEmpty(result.Calories);
                Assert.Equal(4, result.Calories.Count());
            }
        }

        [Fact]
        public async Task ReturnFoodEntriesByThePageSize() {
            var options = new DbContextOptionsBuilder<CalorieTrackerContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var userId = Guid.NewGuid();
            var user = new User {
                Id = userId,
                Email = "test@email.com",
                Name = "user name"
            };

            var foodEntries = Enumerable.Range(0, 20).Select(x =>
                  new FoodEntry {
                      Calorie = 12,
                      FoodName = "food name",
                      Date = DateTime.Today.AddDays(x * -1),
                      User = user
                  });

            using (var context = new CalorieTrackerContext(options)) {
                await context.Users.AddAsync(user);
                await context.FoodEntries.AddRangeAsync(foodEntries);

                await context.SaveChangesAsync();
            }

            using (var context = new CalorieTrackerContext(options)) {
                var getTotalCaloriesByDayHandler = new GetTotalCaloriesByDayHandler(context);
                var getTotalCaloriesByDayRequest = new GetTotalCaloriesByDayRequest(1, 10, userId, DateTime.Today.AddDays(-20), DateTime.Today);

                var result = await getTotalCaloriesByDayHandler.Handle(getTotalCaloriesByDayRequest, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(20, result.TotalCount);
                Assert.NotEmpty(result.Calories);
                Assert.Equal(getTotalCaloriesByDayRequest.PageSize, result.Calories.Count());
            }
        }
    }
}
