using CalorieTracker.Data;
using CalorieTracker.Models.FoodEntries;
using CalorieTracker.Models.FoodEntries.Specifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CalorieTracker.Handlers.FoodEntries {
    public class GetReportHandler : IRequestHandler<GetReportRequest, GetReportResponse> {
        private readonly CalorieTrackerContext context;

        public GetReportHandler(CalorieTrackerContext context) {
            this.context = context;
        }
        public async Task<GetReportResponse> Handle(GetReportRequest request, CancellationToken cancellationToken) {
            var foodEntrySpecification = new DateGreaterThanSpecification(request.To.AddDays(-7))
                                    .And(new DateLessThanSpecification(request.To));

            var count = await context.FoodEntries.Where(foodEntrySpecification.ToExpression()).LongCountAsync(cancellationToken);

            var query = context.FoodEntries.Where(foodEntrySpecification.ToExpression()).Include(f => f.User)
                .GroupBy(f => new { UserId = f.User.Id, UserName = f.User.Name })
                .Select(f => new AverageCalorie { UserName = f.Key.UserName, Calorie = f.Average(x => x.Calorie) });

            var averageCalories = await query.OrderBy(f => f.UserName).Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).AsNoTracking().ToListAsync(cancellationToken);
            
            var totalCount = await query.LongCountAsync(cancellationToken);

            return new GetReportResponse {
                AddedCount = count,
                TotalCount = totalCount,
                AverageCalories = averageCalories
            };
        }
    }
}
