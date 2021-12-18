﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
	public class AppRole : IdentityRole<int>
	{
		public ICollection<AppUserRole> appUserRoles { get; set; }
	}
}