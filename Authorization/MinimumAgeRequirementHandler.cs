﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
	public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
	{
		private readonly ILogger<MinimumAgeRequirementHandler> _logger;

		public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
		{
			_logger = logger;
		}  
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
		{
			var dateOfBirth=DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);
			var userName = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;

			_logger.LogInformation($"User:  {userName}  with date of birth [{dateOfBirth}]");
			if (dateOfBirth.AddYears(requirement.MinimumAge) <  DateTime.Today)
			{
				_logger.LogInformation("Authorization succeeded");
				context.Succeed(requirement);
			}
			else
			{
				_logger.LogInformation("Authorization failed");
			}
			
			return Task.CompletedTask;

		}
	}
}
