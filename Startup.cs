using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Middleware;
using RestaurantAPI.Models.Validators;
using RestaurantAPI.Services;
using RestaurantAPI.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			var authSettings = new AuthenticationSettings();
			Configuration.GetSection("Authentication").Bind(authSettings);
			services.AddSingleton(authSettings);
			services.AddAuthentication(option =>
			{
				option.DefaultAuthenticateScheme = "Bearer";
				option.DefaultScheme = "Bearer";
				option.DefaultChallengeScheme = "Bearer";
			}).AddJwtBearer(cfg =>
			{
				cfg.RequireHttpsMetadata = false;
				cfg.SaveToken = true;
				cfg.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = authSettings.Jwtissuer,
					ValidAudience = authSettings.Jwtissuer,
					IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.JwtKey))

				};

			});
			services.AddAuthorization(option =>
			{
				option.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality"));
				option.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
			});

			services.AddScoped<IAuthorizationHandler, ResourcesOperationRequirementHandler>();
			services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
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
			services.AddCors(options =>
			{
				options.AddPolicy("FrontEndClient", builder =>

					 builder.AllowAnyMethod()
							 .AllowAnyHeader()
							 .WithOrigins("https://localhost:44321"));
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env,RestaurantSeeder seeder)
		{
			app.UseCors("https://localhost:44321");
			seeder.Seed();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseMiddleware<ErrorHandlingMiddleware>();
			app.UseMiddleware<RequestTimeMiddleware>();
			app.UseAuthentication();
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
