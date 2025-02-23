using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Modles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Contexts
{
    internal class webapplicationContext : DbContext
    {
        public webapplicationContext()
        {
            configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }
        public required DbSet<User> Users { get; set; }

        public required DbSet<Right> Rights { get; set; }

        public required DbSet<RightRole> RightRoles { get; set; }

        public required DbSet<Role> Roles { get; set; }
        public required DbSet<UserRole> UserRoles { get; set; }

        public IConfiguration configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("Default");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
