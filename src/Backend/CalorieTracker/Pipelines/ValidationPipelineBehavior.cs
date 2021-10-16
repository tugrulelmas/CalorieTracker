using CalorieTracker.Models;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CalorieTracker.Pipelines {
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly IEnumerable<IValidator<TRequest>> validatiors;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validatiors) {
            this.validatiors = validatiors;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {
            if (validatiors == null) {
                return await next();
            }

           var validationResult = validatiors.Select(v => v.Validate(request)).SelectMany(result => result.Errors).ToList();
            if (!validationResult.Any()) {
                return await next();
            }

            throw new CustomException(string.Join(',', validationResult.Select(e => e.ErrorMessage)));
        }
    }
}
