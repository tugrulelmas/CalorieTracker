using CalorieTracker.Data;
using CalorieTracker.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CalorieTracker.Handlers.Users {
    public class GetUsersHandler : IRequestHandler<GetUsersRequest, IEnumerable<UserDto>> {
        private readonly CalorieTrackerContext context;

        public GetUsersHandler(CalorieTrackerContext context) {
            this.context = context;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersRequest request, CancellationToken cancellationToken) {
            var result = await context.Users.Select(u => new UserDto { Id = u.Id, Email = u.Email, Name = u.Name }).AsNoTracking().ToListAsync();
            return result;
        }
    }
}
