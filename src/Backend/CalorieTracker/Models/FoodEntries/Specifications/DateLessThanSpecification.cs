using System;
using System.Linq.Expressions;

namespace CalorieTracker.Models.FoodEntries.Specifications {
    public class DateLessThanSpecification : Specification<FoodEntry> {
        private readonly DateTime date;

        public DateLessThanSpecification(DateTime date) {
            this.date = date;
        }

        public override Expression<Func<FoodEntry, bool>> ToExpression() {
            if (date == DateTime.MinValue)
                return p => true;

            return p => p.Date <= date;
        }
    }
}
