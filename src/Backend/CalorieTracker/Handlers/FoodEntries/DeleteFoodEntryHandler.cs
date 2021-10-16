using CalorieTracker.Data;
using CalorieTracker.Models;
using CalorieTracker.Models.FoodEntries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CalorieTracker.Handlers.FoodEntries {
    public class DeleteFoodEntryHandler : IRequestHandler<DeleteFoodEntryRequest> {
        private readonly CalorieTrackerContext context;

        public DeleteFoodEntryHandler(CalorieTrackerContext context) {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteFoodEntryRequest request, CancellationToken cancellationToken) {
            var foodEntry = await context.FoodEntries.Where(f => f.Id == request.FoodEntryId && f.User.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

            if (foodEntry is null)
                throw new CustomException(HttpStatusCode.NotFound, "Food entry cannot be found!");

            context.FoodEntries.Remove(foodEntry);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
