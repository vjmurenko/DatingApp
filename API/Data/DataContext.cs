﻿using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
	public class DataContext : IdentityDbContext<AppUser, AppRole, int, 
		IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
		IdentityRoleClaim<int>, IdentityUserToken<int>>
	{
		public DataContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<UserLike> Likes { get; set; }
		public DbSet<Message> Messages { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<AppUser>()
				.HasMany(u => u.AppUserRoles)
				.WithOne(u => u.AppUser)
				.HasForeignKey(u => u.UserId)
				.IsRequired();

			builder.Entity<AppRole>()
				.HasMany(r => r.appUserRoles)
				.WithOne(u => u.AppRole)
				.HasForeignKey(r => r.RoleId)
				.IsRequired();

			builder.Entity<UserLike>()
				.HasKey(l => new { l.SourceUserId, l.LikedUserId });

			builder.Entity<UserLike>()
				.HasOne(s => s.SourceUser)
				.WithMany(l => l.LikedUsers)
				.HasForeignKey(l => l.SourceUserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<UserLike>()
				.HasOne(s => s.LikedUser)
				.WithMany(l => l.LikedByUsers)
				.HasForeignKey(s => s.LikedUserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Message>()
				.HasOne(u => u.Recipient)
				.WithMany(m => m.MessagesReceived)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Message>()
				.HasOne(u => u.Sender)
				.WithMany(m => m.MessagesSent)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}