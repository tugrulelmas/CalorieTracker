using CalorieTracker.ActionDecorators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace CalorieTracker.Controllers {
    [ApiController]
    [Authorize()]
    [Route("v1/[controller]")]
    [ServiceFilter(typeof(ExceptionDecoratorFilter))]
    public class BaseController : ControllerBase {
        protected Guid GetUserId() {
            if (User is null)
                return Guid.Empty;

            if (User.Identity is null)
                return Guid.Empty;

            if (!User.Identity.IsAuthenticated)
                return Guid.Empty;

            var id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(id)) {
                return Guid.Empty;

            }
            return new Guid(id);
        }
    }
}
