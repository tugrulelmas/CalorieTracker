using MediatR;
using System.Collections.Generic;

namespace CalorieTracker.Models.Users {
    public class GetUsersRequest : IRequest<IEnumerable<UserDto>> {
    }
}
