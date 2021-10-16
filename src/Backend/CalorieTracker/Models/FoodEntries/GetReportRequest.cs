using FluentValidation;
using MediatR;
using System;

namespace CalorieTracker.Models.FoodEntries {
    public class GetReportRequest : IRequest<GetReportResponse> {
        public GetReportRequest(int page, int pageSize, DateTime to) {
            To = to.Date;
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; }

        public int PageSize { get; }

        public DateTime To { get; }
    }

    public class GetReportRequestValidator : AbstractValidator<GetReportRequest> {
        public GetReportRequestValidator() {
            RuleFor(r => r.Page).NotEmpty().WithMessage("Page must be greater than 0!");
            RuleFor(r => r.PageSize).NotEmpty().WithMessage("Page size must be greater than 0!");
            RuleFor(r => r.To).NotEmpty().WithMessage("Date must be a valid date!");
            RuleFor(r => r.To).LessThanOrEqualTo(DateTime.Today).WithMessage("Date cannot be greater than today!");
        }
    }
}
