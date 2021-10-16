using MediatR;

namespace CalorieTracker.Models.Users {
    public class GetTokenRequest : IRequest<GetTokenResponse> {
        public string Email { get; set; }
    }
}
