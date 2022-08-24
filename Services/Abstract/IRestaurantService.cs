using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Services.Abstract
{
	public interface IRestaurantService
	{
		RestaurantDto GetById(int id);
		IEnumerable<RestaurantDto> GetAll();
		int Create(CreateRestaurantDto dto, int userId);
		void Delete(int id, ClaimsPrincipal user);
		void Update(int id, UpdateRestaurantDto dto, ClaimsPrincipal user);
	}
}
