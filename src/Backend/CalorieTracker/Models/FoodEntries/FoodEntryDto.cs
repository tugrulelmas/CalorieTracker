using CalorieTracker.Models.Users;

namespace CalorieTracker.Models.FoodEntries {
    public class FoodEntryDto : FoodEntryBase {

        public UserDto User { get; set; }
    }
}
