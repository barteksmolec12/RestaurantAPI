using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entities;
using RestaurantAPI.Middleware;
using RestaurantAPI.Models.Validators;
using RestaurantAPI.Services;
using RestaurantAPI.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers().AddFluentValidation();
			services.AddDbContext<RestaurantDbContext>();
			services.AddScoped<RestaurantSeeder>();
			services.AddScoped<IRestaurantService, RestaurantService>();
			services.AddScoped<IDishService, DishService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddAutoMapper(this.GetType().Assembly);
			services.AddScoped<ErrorHandlingMiddleware>();
			services.AddScoped<RequestTimeMiddleware>();
			services.AddScoped<IPasswordHasher<User>,PasswordHasher<User>> ();
			services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
			services.AddSwaggerGen();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env,RestaurantSeeder seeder)
		{
			seeder.Seed();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseMiddleware<ErrorHandlingMiddleware>();
			app.UseMiddleware<RequestTimeMiddleware>();
			app.UseHttpsRedirection();

			app.UseSwagger();

			app.UseSwaggerUI(c=>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");

			});

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		
		}
	}
}
