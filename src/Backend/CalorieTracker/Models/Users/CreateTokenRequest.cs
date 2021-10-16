using MediatR;

namespace CalorieTracker.Models.Users {
    public class CreateTokenRequest : IRequest<string> {
        public CreateTokenRequest(User user) {
            User = user;
        }

        public User User { get; }
    }
}
