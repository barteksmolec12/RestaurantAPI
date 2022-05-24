using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services.Abstract
{
	public interface IDishService
	{
		int Create(int restaurantId, CreateDishDto dto);
		IEnumerable<DishDto> GetAll(int restaurantId);
		DishDto GetById(int restaurantId, int dishId);
		void Delete(int restaurantId);
	}
}
