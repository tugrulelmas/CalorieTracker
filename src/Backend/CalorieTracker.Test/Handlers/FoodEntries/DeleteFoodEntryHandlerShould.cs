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
    public class DeleteFoodEntryHandlerShould {
        [Fact]
        public async Task ThrowAnExceptionWhenTheFoodEntryDoesntExist() {
            var options = new DbContextOptionsBuilder<CalorieTrackerContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new CalorieTrackerContext(options)) {

                var addFoodEntryHandler = new DeleteFoodEntryHandler(context);
                var addFoodEntryRequest = new DeleteFoodEntryRequest(Guid.NewGuid(), Guid.NewGuid());

                var exception = await Assert.ThrowsAsync<CustomException>(() => addFoodEntryHandler.Handle(addFoodEntryRequest, CancellationToken.None));

                Assert.NotNull(exception);
                Assert.Equal("Food entry cannot be found!", exception.Message);
                Assert.Equal(HttpStatusCode.NotFound, exception.HttpStatusCode);
            }
        }

        [Fact]
        public async Task RemoveFoodEntry() {
            var options = new DbContextOptionsBuilder<CalorieTrackerContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var userId = Guid.NewGuid();

            var user = new User {
                Id = userId,
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
                await context.Users.AddAsync(user);
                await context.FoodEntries.AddAsync(foodEntry);

                await context.SaveChangesAsync();
            }

            using (var context = new CalorieTrackerContext(options)) {

                var addFoodEntryHandler = new DeleteFoodEntryHandler(context);
                var addFoodEntryRequest = new DeleteFoodEntryRequest(userId, foodEntryId);

                await addFoodEntryHandler.Handle(addFoodEntryRequest, CancellationToken.None);
                var dbFoodEntry = await context.FoodEntries.FirstOrDefaultAsync(f => f.Id == foodEntryId);

                Assert.Null(dbFoodEntry);
            }
        }
    }
}
