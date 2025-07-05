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
        public DbSet<User>? Users { get; set; }

        public DbSet<Right>? Rights { get; set; }

        public DbSet<RightRole>? RightRoles { get; set; }

        public DbSet<Role>? Roles { get; set; }
        public DbSet<UserRole>? UserRoles { get; set; }

        public DbSet<New>? News { get; set; }

        public DbSet<NewType>? NewTypes { get; set; }

        public DbSet<BizLog>? BizLogs { get; set; }

        public DbSet<Comment>? Comments { get; set; }

        public DbSet<Weather>? Weathers { get; set; }

        public DbSet<Contact>? Contacts { get; set; }

        public DbSet<Duty>? Dutys { get; set; }

        public DbSet<ContactMessage>? ContactMessages { get; set; }

        public DbSet<Carousel>? Carousels { get; set; }

        public DbSet<ViewLog>? ViewLogs { get; set; }

        public DbSet<City>? Cities { get; set; }

        public IConfiguration configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("Default");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
