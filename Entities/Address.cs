using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Entities
{
	public class Address
	{
		public int Id { get; set; }
		[MaxLength(50)]
		[Required]
		public string City { get; set; }
		[MaxLength(50)]
		[Required]
		public string Street { get; set; }
		public string PostalCode { get; set; }
		public virtual Restaurant Restaurant { get; set; }
	}
}
