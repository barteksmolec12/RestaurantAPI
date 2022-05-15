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
		void Delete(int id);
		void Update(int id, UpdateRestaurantDto dto);
	}
}
