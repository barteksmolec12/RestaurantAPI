﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
	public class AccountService: IAccountService
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IPasswordHasher<User> _passwordHasher;
		private readonly AuthenticationSettings _authenticationSettings;

		public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher,AuthenticationSettings authenticationSettings)
		{
			_dbContext = dbContext;
			_passwordHasher = passwordHasher;
			_authenticationSettings = authenticationSettings;
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

		public string GenerateJwt(LoginUserDto dto)
		{
			//sprawdzenie czy uzytkownik istnieje w bazie danych
			var user = _dbContext
					   .Users
					   .Include(u => u.Role)
				       .FirstOrDefault(u => u.Email == dto.Email);


			if (user is null)
			{
				throw new BadRequestException("Invalid username or password");
			}

			var result=_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

			if(result == PasswordVerificationResult.Failed)
			{
				throw new BadRequestException("Invalid username or password");
			}

			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
				new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}"),
				new Claim(ClaimTypes.Role,$"{user.Role.Name} "),
				new Claim("DateOfBirth",user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
				new Claim("Nationality",user.Nationality)
			};

			if(!string.IsNullOrEmpty(user.Nationality))
			{
				claims.Add(
					new Claim("Nationality",user.Nationality)
					);
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
			var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
			var token = new JwtSecurityToken(_authenticationSettings.Jwtissuer,
										_authenticationSettings.Jwtissuer,
										claims,
										expires: expires,
										signingCredentials: cred);
			var tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.WriteToken(token);
			
		}
	}
}
