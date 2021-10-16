using CalorieTracker.Models.FoodEntries;
using CalorieTracker.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalorieTracker.Controllers {
    public class UsersController : BaseController {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator) {
            this.mediator = mediator;
        }

        [HttpGet("me/foods")]
        public async Task<ActionResult<IEnumerable<FoodEntry>>> GetMyFoods([FromQuery]int page, [FromQuery]int pageSize, [FromQuery] DateTime from, [FromQuery] DateTime to) {
            var userId = GetUserId();
            var result = await mediator.Send(new FilterFoodEntriesRequest(page, pageSize, userId, from, to));
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("foods")]
        public async Task<ActionResult<IEnumerable<FoodEntry>>> GetFoods([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] DateTime from, [FromQuery] DateTime to) {
            var result = await mediator.Send(new FilterFoodEntriesRequest(page, pageSize, Guid.Empty, from, to));
            return Ok(result);
        }

        [HttpGet("me/calories")]
        public async Task<ActionResult<IEnumerable<FoodEntry>>> GetMyCalories([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] DateTime from, [FromQuery] DateTime to) {
            var userId = GetUserId();
            var result = await mediator.Send(new GetTotalCaloriesByDayRequest(page, pageSize, userId, from, to));
            return Ok(result);
        }

        [HttpPost("me/foods")]
        public Task<ActionResult<FoodEntryBase>> AddToMyFoods(FoodEntryServiceRequest request) {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return AddFood();

            async Task<ActionResult<FoodEntryBase>> AddFood() {
                var userId = GetUserId();
                var result = await mediator.Send(new AddFoodEntryRequest(userId, request.Calorie, request.FoodName, request.Date));
                return Ok(result);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{userId}/foods")]
        public Task<ActionResult<FoodEntryBase>> AddFoods(Guid userId, FoodEntryServiceRequest request) {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return AddFood();

            async Task<ActionResult<FoodEntryBase>> AddFood() {
                var result = await mediator.Send(new AddFoodEntryRequest(userId, request.Calorie, request.FoodName, request.Date));
                return Ok(result);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{userId}/foods/{id}")]
        public Task<ActionResult<FoodEntryBase>> UpdateFoodEntry(Guid userId, Guid id, FoodEntryServiceRequest request) {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return AddFood();

            async Task<ActionResult<FoodEntryBase>> AddFood() {
                var result = await mediator.Send(new UpdateFoodEntryRequest(userId, id, request.Calorie, request.FoodName, request.Date));
                return Ok(result);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("foods/report")]
        public async Task<ActionResult<IEnumerable<FoodEntry>>> GetReport([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] DateTime to) {
            var result = await mediator.Send(new GetReportRequest(page, pageSize, to));
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{userId}/foods/{id}")]
        public async Task<ActionResult<IEnumerable<FoodEntry>>> DeleteFoodEntry(Guid userId, Guid id) {
            var result = await mediator.Send(new DeleteFoodEntryRequest(userId, id));
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers() {
            var result = await mediator.Send(new GetUsersRequest());
            return Ok(result);
        }
    }
}
