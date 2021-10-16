using System.Collections.Generic;

namespace CalorieTracker.Models.FoodEntries {
    public class FilterFoodEntriesResponse {
        public long TotalCount { get; set; }

        public IEnumerable<FoodEntryDto> FoodEntries { get; set; }
    }
}
