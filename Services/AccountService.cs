using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Entities;
using RestaurantAPI.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
	public class AccountService: IAccountService
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IPasswordHasher<User> _passwordHasher;

		public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher)
		{
			_dbContext = dbContext;
			_passwordHasher = passwordHasher;
		}
		public void RegisterUser(RegisterUserDto dto)
		{
			var user = new User()
			{
				Email = dto.Email,
				DateOfBirth = dto.DateOfBirth,
				Nationality = dto.Nationality,
				RoleId=dto.RoleId
			};

			var hashedPassword =_passwordHasher.HashPassword(user, dto.Password);
			user.PasswordHash = hashedPassword;
			_dbContext.Users.Add(user);
			_dbContext.SaveChanges();

		}
	}
}
