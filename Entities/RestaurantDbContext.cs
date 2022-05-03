using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Entities
{
	public class RestaurantDbContext:DbContext
	{
		private string _connectionString = "Server=DESKTOP-MGT344C;Database=RestaurantDb;Trusted_Connection=True;";
		public DbSet<Restaurant> Restaurant { get; set; }
		public DbSet<Address> Address { get; set; }
		public DbSet<Dish> Dish { get; set; }

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

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_connectionString);
		}

	}
}
