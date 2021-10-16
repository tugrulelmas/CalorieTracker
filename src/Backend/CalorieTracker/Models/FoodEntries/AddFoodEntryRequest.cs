using FluentValidation;
using MediatR;
using System;

namespace CalorieTracker.Models.FoodEntries {
    public class AddFoodEntryRequest : FoodEntryServiceRequest, IRequest<FoodEntryBase> {
        public AddFoodEntryRequest(Guid userId, int calorie, string foodName, DateTime date) {
            UserId = userId;
            Calorie = calorie;
            FoodName = foodName?.Trim();
            Date = date.Date;
        }

        public Guid UserId { get; }
    }

    public class FoodEntryServiceRequest {
        public int Calorie { get; set; }

        public string FoodName { get; set; }

        public DateTime Date { get; set; }
    }

    public class AddFoodEntryRequestValidator : AbstractValidator<AddFoodEntryRequest> {
        public AddFoodEntryRequestValidator() {
            RuleFor(r => r.Calorie).GreaterThanOrEqualTo(0).WithMessage("Calorie cannot be less than 0!");
            RuleFor(r => r.FoodName).NotEmpty().WithMessage("Food name cannot be empty!");
            RuleFor(r => r.UserId).NotEmpty().WithMessage("User id cannot be empty!");
            RuleFor(r => r.Date).NotEmpty().WithMessage("Date must be valid!");
        }
    }
}
