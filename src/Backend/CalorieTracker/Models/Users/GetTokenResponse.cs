using System;
using System.Collections.Generic;

namespace CalorieTracker.Models.Users {
    public class GetTokenResponse {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int MaximumCalorie { get; set; }

        public string Token { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
