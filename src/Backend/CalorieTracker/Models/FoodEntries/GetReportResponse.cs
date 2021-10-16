using System;
using System.Collections.Generic;

namespace CalorieTracker.Models.FoodEntries {
    public class GetReportResponse {
        public long AddedCount { get; set; }
        public long TotalCount { get; set; }

        public IEnumerable<AverageCalorie> AverageCalories { get; set; }
    }

    public class AverageCalorie {
        public string UserName { get; set; }

        public double Calorie { get; set; }
    }
}
