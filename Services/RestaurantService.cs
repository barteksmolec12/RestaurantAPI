using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
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
		private readonly ILogger <RestaurantService> _logger;


		public RestaurantService(RestaurantDbContext dbContext,IMapper mapper,ILogger<RestaurantService> logger)
		{
			_dbContext = dbContext;
			_mapper = mapper;
			_logger = logger;
		}

		public void Delete (int id)
		{
			_logger.LogWarning($"Restaurant with id: {id} DELETE action invoked");
			var restaurant = _dbContext
				   .Restaurant
				   .FirstOrDefault(r => r.Id == id);

			if (restaurant is null)
				throw new NotFoundException("Restaurant not found");
			
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

		public void Update(int id,UpdateRestaurantDto dto)
		{
			var restById = _dbContext
				   .Restaurant
				   .FirstOrDefault(r => r.Id == id);
			if (restById is null)
				throw new NotFoundException("Restaurant not found");

			restById.Name = dto.Name;
			restById.Description = dto.Description;
			restById.HasDelivery = dto.HasDelivery;

			_dbContext.SaveChanges();
			

		}
	}
}
