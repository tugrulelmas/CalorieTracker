using CalorieTracker.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CalorieTracker.Controllers {
    public class TokensController : BaseController {
        private readonly IMediator mediator;

        public TokensController(IMediator mediator) {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("")]
        public Task<ActionResult<GetTokenResponse>> CreateToken(GetTokenRequest getTokenRequest) {
            if (getTokenRequest is null)
                throw new ArgumentNullException(nameof(getTokenRequest));

            return GetToken();

            async Task<ActionResult<GetTokenResponse>> GetToken() {
                var result = await mediator.Send(getTokenRequest);
                return Ok(result);
            }
        }
    }
}
