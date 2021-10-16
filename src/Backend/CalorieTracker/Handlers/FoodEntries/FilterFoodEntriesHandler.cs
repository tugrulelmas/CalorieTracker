using CalorieTracker.Data;
using CalorieTracker.Models.FoodEntries;
using CalorieTracker.Models.FoodEntries.Specifications;
using CalorieTracker.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CalorieTracker.Handlers.Users {
    public class FilterFoodEntriesHandler : IRequestHandler<FilterFoodEntriesRequest, FilterFoodEntriesResponse> {
        private readonly CalorieTrackerContext context;

        public FilterFoodEntriesHandler(CalorieTrackerContext context) {
            this.context = context;
        }

        public async Task<FilterFoodEntriesResponse> Handle(FilterFoodEntriesRequest request, CancellationToken cancellationToken) {
            var foodEntrySpecification = new UserIdSpecification(request.UserId)
                                    .And(new DateGreaterThanSpecification(request.From))
                                    .And(new DateLessThanSpecification(request.To));

            var query = context.FoodEntries.Where(foodEntrySpecification.ToExpression());

            var totalCount = await query.LongCountAsync();

            var foodEntries = await query.OrderByDescending(f => f.Date).ThenBy(f=>f.User.Name).Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
                .Include(f => f.User).AsNoTracking().ToListAsync();

            if (foodEntries == null) {
                return new FilterFoodEntriesResponse {
                    TotalCount = totalCount
                };
            }

            var entries = foodEntries.Select(f => new FoodEntryDto {
                Calorie = f.Calorie,
                Date = f.Date,
                FoodName = f.FoodName,
                Id = f.Id,
                User = new UserDto {
                    Id = f.User.Id,
                    Email = f.User.Email,
                    Name = f.User.Name
                }
            });

            return new FilterFoodEntriesResponse {
                TotalCount = totalCount,
                FoodEntries = entries
            };
        }
    }
}
