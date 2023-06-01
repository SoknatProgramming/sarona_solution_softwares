using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sarona_Solution_Softwares.Data
{
	public class AuthDBContext : IdentityDbContext
	{
		public AuthDBContext(DbContextOptions<AuthDBContext> options): base(options)
		{

		}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            var readerRoleId = "cec4fada-2834-4b74-b019-a5772398b17";
            var writerRoleId = "256ca52b-39f1-43b7-9ad3-0ea71d04a4e";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()

                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()

                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}

