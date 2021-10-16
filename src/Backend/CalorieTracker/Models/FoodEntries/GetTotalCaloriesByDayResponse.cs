using System;
using System.Collections.Generic;

namespace CalorieTracker.Models.FoodEntries {
    public class GetTotalCaloriesByDayResponse {
        public long TotalCount { get; set; }

        public IEnumerable<TotalCaloriesByDay> Calories { get; set; }
    }

    public class TotalCaloriesByDay {
        public int Calorie { get; set; }

        public DateTime Date { get; set; }
    }
}
