using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantAPI.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
	
	public class RestaurantService: IRestaurantService
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IMapper _mapper;
		private readonly ILogger <RestaurantService> _logger;
		private readonly IAuthorizationService _authorizationService;

		public RestaurantService(RestaurantDbContext dbContext,IMapper mapper,ILogger<RestaurantService> logger,IAuthorizationService authorizationService)
		{
			_dbContext = dbContext;
			_mapper = mapper;
			_logger = logger;
			_authorizationService = authorizationService;
		}

		public void Delete (int id, ClaimsPrincipal user)
		{
			_logger.LogWarning($"Restaurant with id: {id} DELETE action invoked");
			var restaurant = _dbContext
				   .Restaurant
				   .FirstOrDefault(r => r.Id == id);

			if (restaurant is null)
				throw new NotFoundException("Restaurant not found");

			var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant, new ResourcesOperationRequirement(ResourceOperation.Delete)).Result;

			if (!authorizationResult.Succeeded)
			{
				throw new ForbidException();
			}

			_dbContext.Restaurant.Remove(restaurant);
			_dbContext.SaveChanges();
		
		
		}
		public RestaurantDto GetById(int id)
		{
			var restaurant = _dbContext
				   .Restaurant
				   .Include(r => r.Address)
				   .Include(r => r.Dishes)
				   .FirstOrDefault(r => r.Id == id);

			if (restaurant is null) 
				throw new NotFoundException("Restaurant not found");
			var result = _mapper.Map<RestaurantDto>(restaurant);
			return result;

		}
		public IEnumerable<RestaurantDto> GetAll(RestaurantQuery query)

		{
			var restaurants = _dbContext
				.Restaurant
				.Include(r => r.Address)
				.Include(r => r.Dishes)
				.Where(r => query.SearchPhrase == null || (r.Name.ToLower().Contains(query.SearchPhrase.ToLower())
														|| r.Description.ToLower().Contains(query.SearchPhrase.ToLower())))
				.Skip(query.PageSize * query.PageNumber - 1)
				.Take(query.PageSize)
				.ToList();

			var result = _mapper.Map<List<RestaurantDto>>(restaurants);
			return result;
		}
		public int Create(CreateRestaurantDto dto,int userId)
		{
			var restaurant = _mapper.Map<Restaurant>(dto);
			restaurant.CreateById = userId;
			_dbContext.Restaurant.Add(restaurant);
			_dbContext.SaveChanges();
			return restaurant.Id;
		}

		public void Update(int id,UpdateRestaurantDto dto,ClaimsPrincipal user)
		{
			
			var restById = _dbContext
				   .Restaurant
				   .FirstOrDefault(r => r.Id == id);

			
			if (restById is null)
				throw new NotFoundException("Restaurant not found");

			var authorizationResult=_authorizationService.AuthorizeAsync(user, restById, new ResourcesOperationRequirement(ResourceOperation.Update)).Result;

			if (!authorizationResult.Succeeded)
			{
				throw new ForbidException();
			}

			restById.Name = dto.Name;
			restById.Description = dto.Description;
			restById.HasDelivery = dto.HasDelivery;

			_dbContext.SaveChanges();
			

		}
	}
}
