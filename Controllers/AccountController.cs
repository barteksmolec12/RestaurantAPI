using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{ 
	[Route("api/account")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}
		[HttpPost("register")]
		public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
		{
			_accountService.RegisterUser(dto);
			return Ok();
		}
	}
}
