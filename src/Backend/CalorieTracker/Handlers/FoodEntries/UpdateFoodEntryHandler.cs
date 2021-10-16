using CalorieTracker.Data;
using CalorieTracker.Models;
using CalorieTracker.Models.FoodEntries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CalorieTracker.Handlers.FoodEntries {
    public class UpdateFoodEntryHandler : IRequestHandler<UpdateFoodEntryRequest> {
        private readonly CalorieTrackerContext context;

        public UpdateFoodEntryHandler(CalorieTrackerContext context) {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateFoodEntryRequest request, CancellationToken cancellationToken) {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user is null)
                throw new CustomException("Invalid user id");

            var foodEntry = await context.FoodEntries.FirstOrDefaultAsync(u => u.Id == request.FoodEntryId);

            if (foodEntry is null)
                throw new CustomException("Invalid id");

            foodEntry.Calorie = request.Calorie;
            foodEntry.Date = request.Date;
            foodEntry.FoodName = request.FoodName;
            foodEntry.User = user;

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
