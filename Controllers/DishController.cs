using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
	[Route("api/restaurant/{restaurantId}/dish")]

	[ApiController] //automatyczne walidowanie każdej akcji
	public class DishController : ControllerBase
	{
		private readonly IDishService _dishService;

		public DishController(IDishService dishService)
		{
			_dishService = dishService;
		}
		[HttpPost]
		public ActionResult Post([FromRoute] int restaurantId,[FromBody] CreateDishDto dto )
		{
			var newDishId=_dishService.Create(restaurantId, dto);
			return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);

		}

		[HttpGet] //wszystkie dania dla konkretnej restauracji
		public ActionResult GetAll([FromRoute] int restaurantId)
		{

			var dishes = _dishService.GetAll(restaurantId);
			return Ok(dishes);


		}

		[HttpGet("{dishId}")] //1 danie
		public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
		{

			DishDto dish = _dishService.GetById(restaurantId,dishId);
			return Ok(dish);


		}
		[HttpDelete] //usuniecie wszystkich dań dla restauracji
		public ActionResult<DishDto> Delete([FromRoute] int restaurantId)
		{

			_dishService.Delete(restaurantId);

			return NoContent();


		}
	}
}
