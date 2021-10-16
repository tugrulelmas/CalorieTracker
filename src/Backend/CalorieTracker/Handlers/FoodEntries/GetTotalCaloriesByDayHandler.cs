using CalorieTracker.Data;
using CalorieTracker.Models.FoodEntries;
using CalorieTracker.Models.FoodEntries.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CalorieTracker.Handlers.FoodEntries {
    public class GetTotalCaloriesByDayHandler : IRequestHandler<GetTotalCaloriesByDayRequest, GetTotalCaloriesByDayResponse> {
        private readonly CalorieTrackerContext context;

        public GetTotalCaloriesByDayHandler(CalorieTrackerContext context) {
            this.context = context;
        }

        public async Task<GetTotalCaloriesByDayResponse> Handle(GetTotalCaloriesByDayRequest request, CancellationToken cancellationToken) {
            var foodEntrySpecification = new UserIdSpecification(request.UserId)
                                    .And(new DateGreaterThanSpecification(request.From))
                                    .And(new DateLessThanSpecification(request.To));

            var query = context.FoodEntries.Where(foodEntrySpecification.ToExpression()).GroupBy(f => f.Date)
                .Select(f => new TotalCaloriesByDay { Date = f.Key, Calorie = f.Sum(x => x.Calorie) });
            
            var totalCount = await query.LongCountAsync(cancellationToken);

            var calories = await query.OrderByDescending(f => f.Date).Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToListAsync(cancellationToken);

            return new GetTotalCaloriesByDayResponse {
                TotalCount = totalCount,
                Calories = calories
            };
        }
    }
}
