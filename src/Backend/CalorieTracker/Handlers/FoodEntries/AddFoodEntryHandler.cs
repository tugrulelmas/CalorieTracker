using CalorieTracker.Data;
using CalorieTracker.Models;
using CalorieTracker.Models.FoodEntries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CalorieTracker.Handlers.Users {
    public class AddFoodEntryHandler : IRequestHandler<AddFoodEntryRequest, FoodEntryBase> {
        private readonly CalorieTrackerContext context;

        public AddFoodEntryHandler(CalorieTrackerContext context) {
            this.context = context;
        }

        public async Task<FoodEntryBase> Handle(AddFoodEntryRequest request, CancellationToken cancellationToken) {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
                throw new CustomException("Invalid user id");

            var foodEntry = new FoodEntry {
                Calorie = request.Calorie,
                Date = request.Date,
                FoodName = request.FoodName,
                Id = Guid.NewGuid(),
                User = user
            };

            await context.FoodEntries.AddAsync(foodEntry);

            await context.SaveChangesAsync();

            return new FoodEntryBase {
                Id = foodEntry.Id,
                Calorie = foodEntry.Calorie,
                Date = foodEntry.Date,
                FoodName = foodEntry.FoodName
            };
        }
    }
}
