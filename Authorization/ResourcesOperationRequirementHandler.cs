﻿using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
	public class ResourcesOperationRequirementHandler : AuthorizationHandler<ResourcesOperationRequirement, Restaurant>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourcesOperationRequirement requirement, Restaurant restaurant)
		{
			if (requirement.ResourceOperation == ResourceOperation.Read || requirement.ResourceOperation == ResourceOperation.Create)
			{
				context.Succeed(requirement);
			}

			var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

			if (restaurant.CreateById == int.Parse(userId))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
