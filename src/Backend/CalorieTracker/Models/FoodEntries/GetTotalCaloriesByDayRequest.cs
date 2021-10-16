using FluentValidation;
using MediatR;
using System;

namespace CalorieTracker.Models.FoodEntries {
    public class GetTotalCaloriesByDayRequest : IRequest<GetTotalCaloriesByDayResponse> {
        public GetTotalCaloriesByDayRequest(int page, int pageSize, Guid userId, DateTime from, DateTime to) {
            Page = page;
            PageSize = pageSize;
            UserId = userId;
            From = from;
            To = to;
        }

        public int Page { get; }

        public int PageSize { get; }

        public Guid UserId { get; }

        public DateTime From { get; }

        public DateTime To { get; }
    }

    public class GetTotalCaloriesByDayRequestValidator : AbstractValidator<GetTotalCaloriesByDayRequest> {
        public GetTotalCaloriesByDayRequestValidator() {
            RuleFor(r => r.Page).NotEmpty().WithMessage("Page must be greater than 0!");
            RuleFor(r => r.PageSize).NotEmpty().WithMessage("Page size must be greater than 0!");
            RuleFor(r => r.UserId).NotEmpty().WithMessage("User id cannot be empty!");
            RuleFor(r => r.To).LessThanOrEqualTo(DateTime.Today).WithMessage("To date cannot be greater than today!");
            RuleFor(r => r.From).LessThanOrEqualTo(DateTime.Today).WithMessage("From cannot be greater than today!");
            RuleFor(r => r.From).LessThanOrEqualTo(r => r.To).When(r => r.To > DateTime.MinValue).WithMessage("From date must be less than To date!");
        }
    }
}
