using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Services.Abstract;

namespace RestaurantAPI.Controllers
{
	[Route("api/restaurant")]
	public class RestaurantController : ControllerBase
	{
		private readonly IRestaurantService _restaurantService;

		[HttpDelete("{id}")]
		public ActionResult Delete ([FromRoute]int id)
		{
			var isDeleted=_restaurantService.Delete(id);

			if (isDeleted) return NoContent();
			return NotFound();
		}
		public RestaurantController(IRestaurantService restaurantService)
		{
			_restaurantService = restaurantService;
		}
		[HttpPost]
		public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
		{
			if(! ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			int id=_restaurantService.Create(dto);
			return Created($"/api/restaurant/{id}",null); //informacja że utworzono zasób na serwerze
		}
		[HttpGet]
		public ActionResult<IEnumerable<RestaurantDto>> GetAll()
		{
			var restaurantsDtos = _restaurantService.GetAll();	
			return Ok(restaurantsDtos);
		}
		[Route("api/restaurant/{id}")]
		[HttpGet("{id}")]
		public ActionResult<RestaurantDto> Get([FromRoute]int id)
		{
			var restaurantDto = _restaurantService.GetById(id);
		
			if(restaurantDto is null)
			{
				return NotFound();
			}

			return Ok(restaurantDto);
		}
	}
}
