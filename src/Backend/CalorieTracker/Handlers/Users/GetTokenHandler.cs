using CalorieTracker.Data;
using CalorieTracker.Models;
using CalorieTracker.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CalorieTracker.Handlers.Users {
    public class GetTokenHandler : IRequestHandler<GetTokenRequest, GetTokenResponse> {
        private readonly CalorieTrackerContext context;
        private readonly IMediator mediator;

        public GetTokenHandler(CalorieTrackerContext context, IMediator mediator) {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<GetTokenResponse> Handle(GetTokenRequest request, CancellationToken cancellationToken) {
            var user = await context.Users.Where(u => u.Email == request.Email).Include(u => u.Roles).AsNoTracking().FirstOrDefaultAsync();
            if (user is null)
                throw new CustomException($"Invalid credentials");

            var token = await mediator.Send(new CreateTokenRequest(user));

            return new GetTokenResponse {
                Email = user.Email,
                Id = user.Id,
                MaximumCalorie = user.MaximumCalorie,
                Name = user.Name,
                Roles = user.Roles.Select(r => r.Name),
                Token = token
            };
        }
    }
}
