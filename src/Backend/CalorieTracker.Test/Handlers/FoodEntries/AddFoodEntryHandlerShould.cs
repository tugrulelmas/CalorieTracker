using CalorieTracker.Data;
using CalorieTracker.Handlers.FoodEntries;
using CalorieTracker.Models;
using CalorieTracker.Models.FoodEntries;
using CalorieTracker.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CalorieTracker.Test.Handlers.FoodEntries {
    public class AddFoodEntryHandlerShould {
        [Fact]
        public async Task ThrowAnExceptionWhenTheUserIsNotValid() {
            var options = new DbContextOptionsBuilder<CalorieTrackerContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new CalorieTrackerContext(options)) {
                var addFoodEntryHandler = new AddFoodEntryHandler(context);
                var addFoodEntryRequest = new AddFoodEntryRequest(Guid.NewGuid(), 0, "food name", DateTime.Today);

                var exception = await Assert.ThrowsAsync<CustomException>(() => addFoodEntryHandler.Handle(addFoodEntryRequest, CancellationToken.None));

                Assert.NotNull(exception);
                Assert.Equal("Invalid user id", exception.Message);
                Assert.Equal(HttpStatusCode.BadRequest, exception.HttpStatusCode);
            }
        }

        [Fact]
        public async Task AddFoodEntry() {
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
                var addFoodEntryHandler = new AddFoodEntryHandler(context);
                var addFoodEntryRequest = new AddFoodEntryRequest(userId, 12, "food name", DateTime.Today);

                var result = await addFoodEntryHandler.Handle(addFoodEntryRequest, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(addFoodEntryRequest.Calorie, result.Calorie);
                Assert.Equal(addFoodEntryRequest.Date, result.Date);
                Assert.Equal(addFoodEntryRequest.FoodName, result.FoodName);
            }
        }
    }
}
