using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
	public class RestaurantService: IRestaurantService
	{
		private readonly RestaurantDbContext _dbContext;
		private readonly IMapper _mapper;

		public RestaurantService(RestaurantDbContext dbContext,IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public bool Delete (int id)
		{
			var restaurant = _dbContext
				   .Restaurant
				   .FirstOrDefault(r => r.Id == id);

			if (restaurant is null) return false;
			_dbContext.Restaurant.Remove(restaurant);
			_dbContext.SaveChanges();
			return true;

		
		}
		public RestaurantDto GetById(int id)
		{
			var restaurant = _dbContext
				   .Restaurant
				   .Include(r => r.Address)
				   .Include(r => r.Dishes)
				   .FirstOrDefault(r => r.Id == id);

			if (restaurant is null) return null;
			var result = _mapper.Map<RestaurantDto>(restaurant);
			return result;

		}
		public IEnumerable<RestaurantDto> GetAll()

		{
			var restaurants = _dbContext
				.Restaurant
				.Include(r => r.Address)
				.Include(r => r.Dishes)
				.ToList();

			var result = _mapper.Map<List<RestaurantDto>>(restaurants);
			return result;
		}
		public int Create(CreateRestaurantDto dto)
		{
			var restaurant = _mapper.Map<Restaurant>(dto);
			_dbContext.Restaurant.Add(restaurant);
			_dbContext.SaveChanges();
			return restaurant.Id;
		}
	}
}
