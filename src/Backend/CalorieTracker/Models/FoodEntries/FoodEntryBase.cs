using System;

namespace CalorieTracker.Models.FoodEntries {
    public class FoodEntryBase {
        public Guid Id { get; set; }

        public int Calorie { get; set; }

        public string FoodName { get; set; }

        public DateTime Date { get; set; }
    }
}
