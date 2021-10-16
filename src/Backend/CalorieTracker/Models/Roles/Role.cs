using CalorieTracker.Models.Users;
using System;
using System.Collections.Generic;

namespace CalorieTracker.Models.Roles {
    public class Role {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
