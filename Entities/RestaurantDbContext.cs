using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Entities
{
	public class RestaurantDbContext:DbContext
	{
		public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
		{

		}
		public DbSet<Restaurant> Restaurant { get; set; }
		public DbSet<Address> Address { get; set; }
		public DbSet<Dish> Dish { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Restaurant>()
				.Property(r => r.Name)
				.IsRequired()
				.HasMaxLength(25);

			modelBuilder.Entity<Dish>()
				.Property(r => r.Name)
				.IsRequired();

			
		}

	}
}
