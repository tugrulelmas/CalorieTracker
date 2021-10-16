using CalorieTracker.Data;
using CalorieTracker.Handlers.FoodEntries;
using CalorieTracker.Models;
using CalorieTracker.Models.FoodEntries;
using CalorieTracker.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CalorieTracker.Test.Handlers.FoodEntries {
    public class UpdateFoodEntryHandlerShould {
        [Fact]
        public async Task ThrowAnExceptionWhenTheUserDoesntExist() {
            var options = new DbContextOptionsBuilder<CalorieTrackerContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new CalorieTrackerContext(options)) {
                var updateFoodEntryHandler = new UpdateFoodEntryHandler(context);
                var updateFoodEntryRequest = new UpdateFoodEntryRequest(Guid.NewGuid(), Guid.NewGuid(), 0, "test", DateTime.Today);

                var exception = await Assert.ThrowsAsync<CustomException>(() => updateFoodEntryHandler.Handle(updateFoodEntryRequest, CancellationToken.None));

                Assert.NotNull(exception);
                Assert.Equal("Invalid user id", exception.Message);
                Assert.Equal(HttpStatusCode.BadRequest, exception.HttpStatusCode);
            }
        }

        [Fact]
        public async Task ThrowAnExceptionWhenTheFoodEntryDoesntExist() {
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
                var updateFoodEntryHandler = new UpdateFoodEntryHandler(context);
                var updateFoodEntryRequest = new UpdateFoodEntryRequest(userId, Guid.NewGuid(), 0, "test", DateTime.Today);

                var exception = await Assert.ThrowsAsync<CustomException>(() => updateFoodEntryHandler.Handle(updateFoodEntryRequest, CancellationToken.None));

                Assert.NotNull(exception);
                Assert.Equal("Invalid id", exception.Message);
                Assert.Equal(HttpStatusCode.BadRequest, exception.HttpStatusCode);
            }
        }

        [Fact]
        public async Task UpdateFoodEntry() {
            var options = new DbContextOptionsBuilder<CalorieTrackerContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var userId = Guid.NewGuid();

            var user = new User {
                Id = userId,
                Email = "test@email.com",
                Name = "user name"
            };

            var user2Id = Guid.NewGuid();
            var user2 = new User {
                Id = user2Id,
                Email = "test@email.com",
                Name = "user name"
            };

            var foodEntryId = Guid.NewGuid();
            var foodEntry = new FoodEntry {
                Id = foodEntryId,
                Calorie = 12,
                FoodName = "food name",
                Date = DateTime.Today,
                User = user
            };

            using (var context = new CalorieTrackerContext(options)) {
                await context.Users.AddRangeAsync(user, user2);
                await context.FoodEntries.AddAsync(foodEntry);

                await context.SaveChangesAsync();
            }

            using (var context = new CalorieTrackerContext(options)) {
                var updateFoodEntryHandler = new UpdateFoodEntryHandler(context);
                var updateFoodEntryRequest = new UpdateFoodEntryRequest(user2Id, foodEntryId, 18, "new name", DateTime.Today);

                await updateFoodEntryHandler.Handle(updateFoodEntryRequest, CancellationToken.None);
                var dbFoodEntry = await context.FoodEntries.FirstOrDefaultAsync(f => f.Id == foodEntryId);

                Assert.NotNull(dbFoodEntry);
                Assert.Equal(updateFoodEntryRequest.Calorie, dbFoodEntry.Calorie);
                Assert.Equal(updateFoodEntryRequest.Date, dbFoodEntry.Date);
                Assert.Equal(updateFoodEntryRequest.FoodName, dbFoodEntry.FoodName);
                Assert.Equal(user2Id, dbFoodEntry.User.Id);
            }
        }
    }
}
