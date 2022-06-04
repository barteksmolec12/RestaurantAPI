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
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Controllers
{
	[Route("api/restaurant")]
	[Consumes("application/json")]
	[ApiController]
	[Authorize]

	public class RestaurantController : ControllerBase
	{
		private readonly IRestaurantService _restaurantService;

		[HttpPut("{id}")]
		public ActionResult Update([FromBody] UpdateRestaurantDto dto,[FromRoute] int id)
		{
			_restaurantService.Update(id, dto);	
			return Ok();	
		}


		[HttpDelete("{id}")]
		public ActionResult Delete ([FromRoute]int id)
		{
			_restaurantService.Delete(id);

			return NoContent();
		}


		public RestaurantController(IRestaurantService restaurantService)
		{
			_restaurantService = restaurantService;
		}


		[HttpPost]
		[Authorize(Roles = "Menager")]
		public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
		{
			int id=_restaurantService.Create(dto);
			return Created($"/api/restaurant/{id}",null); //informacja że utworzono zasób na serwerze
		}


		[HttpGet]
		[Authorize(Policy = "Atleast20")] //własna polityka utworzona w startup.cs
		public ActionResult<IEnumerable<RestaurantDto>> GetAll()
		{
			
			var restaurantsDtos = _restaurantService.GetAll();	
			return Ok(restaurantsDtos);
		}


		
		[HttpGet("{id}")]
		[AllowAnonymous] //zapytania bez nagłówka autoryzacji
		public ActionResult<RestaurantDto> Get([FromRoute]int id)
		{
			var restaurantDto = _restaurantService.GetById(id);
			return Ok(restaurantDto);
		}
	}
}
