using FluentValidation;
using MediatR;
using System;

namespace CalorieTracker.Models.FoodEntries {
    public class UpdateFoodEntryRequest : FoodEntryServiceRequest, IRequest {
        public UpdateFoodEntryRequest(Guid userId, Guid foodEntryId, int calorie, string foodName, DateTime date) {
            UserId = userId;
            FoodEntryId = foodEntryId;
            Calorie = calorie;
            FoodName = foodName?.Trim();
            Date = date.Date;
        }

        public Guid UserId { get; }

        public Guid FoodEntryId { get; }
    }

    public class UpdateFoodEntryRequestValidator : AbstractValidator<UpdateFoodEntryRequest> {
        public UpdateFoodEntryRequestValidator() {
            RuleFor(r => r.Calorie).GreaterThanOrEqualTo(0).WithMessage("Calorie cannot be less than 0!");
            RuleFor(r => r.FoodName).NotEmpty().WithMessage("Food name cannot be empty!");
            RuleFor(r => r.UserId).NotEmpty().WithMessage("User id cannot be empty!");
            RuleFor(r => r.FoodEntryId).NotEmpty().WithMessage("Id cannot be empty!");
            RuleFor(r => r.Date).NotEmpty().WithMessage("Date must be valid!");
        }
    }
}
