﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI
{
	public class AuthenticationSettings
	{
		public string JwtKey { get; set; }
		public int JwtExpireDays { get; set; }
		public string Jwtissuer { get; set; }
	}
}
