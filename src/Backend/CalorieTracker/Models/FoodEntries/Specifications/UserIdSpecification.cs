using System;
using System.Linq.Expressions;

namespace CalorieTracker.Models.FoodEntries.Specifications {
    public class UserIdSpecification : Specification<FoodEntry> {
        private readonly Guid userId;

        public UserIdSpecification(Guid userId) {
            this.userId = userId;
        }

        public override Expression<Func<FoodEntry, bool>> ToExpression() {
            if (userId == Guid.Empty)
                return p => true;

            return p => p.User.Id == userId;
        }
    }
}
