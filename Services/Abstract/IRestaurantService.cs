using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services.Abstract
{
	public interface IRestaurantService
	{
		RestaurantDto GetById(int id);
		IEnumerable<RestaurantDto> GetAll();
		int Create(CreateRestaurantDto dto);
	}
}
