using System;
using Microsoft.EntityFrameworkCore;
using Sarona_Solution_Softwares.Model.Domain;

namespace Sarona_Solution_Softwares.Data
{
	public class UserDBContext : DbContext
	{
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {

        }
        public DbSet<Image> Images { get; set; }
    }
}

