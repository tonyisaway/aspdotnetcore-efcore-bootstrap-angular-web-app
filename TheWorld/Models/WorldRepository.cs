namespace TheWorld.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Extensions.Logging;

    public class WorldRepository : IWorldRepository
    {
        private readonly WorldContext context;

        private readonly ILogger<WorldRepository> logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            this.logger.LogInformation("Getting All Trips from the Database");
            return this.context.Trips.ToList();
        }
    }
}