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
using System.Security.Claims;
using RestaurantAPI.Models;

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
			_restaurantService.Update(id, dto,User);	
			return Ok();	
		}


		[HttpDelete("{id}")]
		public ActionResult Delete ([FromRoute]int id)
		{
			_restaurantService.Delete(id,User);

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
			var userId=int.Parse(User.FindFirst(c=>c.Type == ClaimTypes.NameIdentifier).Value);
			int id=_restaurantService.Create(dto, userId);
			return Created($"/api/restaurant/{id}",null); //informacja że utworzono zasób na serwerze
		}


		[HttpGet]
		[Authorize(Policy = "Atleast20")] //własna polityka utworzona w startup.cs
		public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery] RestaurantQuery query)
		{
			
			var restaurantsDtos = _restaurantService.GetAll(query);	
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
