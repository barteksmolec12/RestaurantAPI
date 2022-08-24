using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
	public enum ResourceOperation
	{
		Create,
		Read,
		Update,
		Delete
	}

	public class ResourcesOperationRequirement:IAuthorizationRequirement
	{
		public ResourcesOperationRequirement(ResourceOperation resourceOperation)
		{
			ResourceOperation = resourceOperation;
		}
		public ResourceOperation ResourceOperation { get; set; }
	}
}
