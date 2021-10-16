using CalorieTracker.Models.Users;

namespace CalorieTracker.Models.FoodEntries {
    public class FoodEntry : FoodEntryBase {

        public User User { get; set; }
    }
}
