using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantAPI.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
	public class DishService:IDishService
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IMapper _mapper;

		public DishService(RestaurantDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		
		}
		public int Create (int restaurantId,CreateDishDto dto)
		{
			var restaurant = _dbContext
			   .Restaurant
			   .FirstOrDefault(r => r.Id == restaurantId);

			if (restaurant is null)
				throw new NotFoundException("Restaurant not found");

			var dishEntity = _mapper.Map<Dish>(dto);
			dishEntity.RestaurantId = restaurantId;
			_dbContext.Dish.Add(dishEntity);
			_dbContext.SaveChanges();
			return dishEntity.Id;
		}

		public IEnumerable<DishDto> GetAll(int restaurantId)
		{
			var restaurant = _dbContext
			   .Restaurant
			   .FirstOrDefault(r => r.Id == restaurantId);

			if (restaurant is null)
				throw new NotFoundException("Restaurant not found");

			var dishes = _dbContext.Dish.Where(d => d.RestaurantId == restaurantId).ToList();
			var result = _mapper.Map<List<DishDto>>(dishes);
			return result;

		}

		public DishDto GetById(int restaurantId, int dishId)
		{
			#region Check if Restaurant Exists
			var restaurant = _dbContext
			   .Restaurant
			   .FirstOrDefault(r => r.Id == restaurantId);

			if (restaurant is null)
				throw new NotFoundException("Restaurant not found");
			#endregion


			#region Find dish by ID
			var dish = _dbContext.Dish.Where(d => d.RestaurantId == restaurantId && d.Id == dishId).FirstOrDefault();
			if (dish is null)
				throw new NotFoundException("Dish not found");

			var result = _mapper.Map<DishDto>(dish);
			return result;
			#endregion

		}

		public void Delete(int restaurantId)
		{
			
			var restaurant = _dbContext
				   .Restaurant
				   .FirstOrDefault(r => r.Id == restaurantId);

			if (restaurant is null)
				throw new NotFoundException("Restaurant not found");

			_dbContext.Dish.RemoveRange(_dbContext.Dish.Where(x => x.RestaurantId == restaurantId));
			_dbContext.SaveChanges();
		}
	}
}
