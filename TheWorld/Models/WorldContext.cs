using Microsoft.EntityFrameworkCore;

namespace TheWorld.Models
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class WorldContext : IdentityDbContext<WorldUser>
    {
        private readonly IConfigurationRoot config;

        public WorldContext(IConfigurationRoot config, DbContextOptions options) 
            : base(options)
        {
            this.config = config;
        }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<Stop> Stops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(this.config["ConnectionStrings:WorldContextConnection"]);
        }
    }
}
