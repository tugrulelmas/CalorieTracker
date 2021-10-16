using System;
using System.Linq.Expressions;

namespace CalorieTracker.Models.FoodEntries.Specifications {
    public class DateGreaterThanSpecification : Specification<FoodEntry> {
        private readonly DateTime date;

        public DateGreaterThanSpecification(DateTime date) {
            this.date = date;
        }

        public override Expression<Func<FoodEntry, bool>> ToExpression() {
            if (date == DateTime.MinValue)
                return p => true;

            return p => p.Date >= date;
        }
    }
}
