using FluentValidation;
using MediatR;
using System;

namespace CalorieTracker.Models.FoodEntries {
    public class FilterFoodEntriesRequest : IRequest<FilterFoodEntriesResponse> {
        public FilterFoodEntriesRequest(int page, int pageSize, Guid userId, DateTime from, DateTime to) {
            Page = page;
            PageSize = pageSize;
            UserId = userId;
            From = from.Date;
            To = to.Date;
        }

        public int Page { get; }

        public int PageSize { get; }

        public Guid UserId { get; }

        public DateTime From { get; }

        public DateTime To { get; }
    }

    public class FilterFoodEntriesRequestValidator : AbstractValidator<FilterFoodEntriesRequest> {
        public FilterFoodEntriesRequestValidator() {
            RuleFor(r => r.Page).NotEmpty().WithMessage("Page must be greater than 0!");
            RuleFor(r => r.PageSize).NotEmpty().WithMessage("Page size must be greater than 0!");
            RuleFor(r => r.To).LessThanOrEqualTo(DateTime.Today).WithMessage("To cannot be greater than today!");
            RuleFor(r => r.From).LessThanOrEqualTo(DateTime.Today).WithMessage("From cannot be greater than today!");
            RuleFor(r => r.From).LessThanOrEqualTo(r => r.To).When(r => r.To > DateTime.MinValue).WithMessage("From must be less than To!");
        }
    }
}
