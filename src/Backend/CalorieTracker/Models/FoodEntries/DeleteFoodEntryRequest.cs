using FluentValidation;
using MediatR;
using System;

namespace CalorieTracker.Models.FoodEntries {
    public class DeleteFoodEntryRequest: IRequest {
        public DeleteFoodEntryRequest(Guid userId, Guid foodEntryId) {
            UserId = userId;
            FoodEntryId = foodEntryId;
        }

        public Guid UserId { get; }
        public Guid FoodEntryId { get; }
    }

    public class DeleteFoodEntryRequestValidator : AbstractValidator<DeleteFoodEntryRequest> {
        public DeleteFoodEntryRequestValidator() {
            RuleFor(r => r.UserId).NotEmpty().WithMessage("User id cannot be empty!");
            RuleFor(r => r.FoodEntryId).NotEmpty().WithMessage("Food entry id cannot be empty!");
        }
    }
}
