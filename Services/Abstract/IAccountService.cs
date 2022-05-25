using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services.Abstract
{
	public interface IAccountService
	{
		 void RegisterUser(RegisterUserDto dto);
	}
}
