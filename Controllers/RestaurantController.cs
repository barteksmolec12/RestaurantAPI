using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
	[Route("api/restaurant")]
	public class RestaurantController : ControllerBase
	{
		private readonly RestaurantDbContext _dbContext;
		public RestaurantController(RestaurantDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		[HttpGet]
		public ActionResult<IEnumerable<Restaurant>> GetAll()
		{
			var restaurants = _dbContext.Restaurant.ToList();
			return Ok(restaurants);
		}
		[Route("api/restaurant/{id}")]
		[HttpGet("{id}")]
		public ActionResult<Restaurant> Get([FromRoute]int id)
		{
			var restaurant = _dbContext.Restaurant.FirstOrDefault(r => r.Id == id);
			if(restaurant is null)
			{
				return NotFound();
			}
			
			return Ok(restaurant);
		}
	}
}
