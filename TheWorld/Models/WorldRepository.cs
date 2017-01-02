namespace TheWorld.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
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

        public void AddTrip(Trip trip)
        {
            this.context.Trips.Add(trip);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await this.context.SaveChangesAsync()) > 0;
        }

        public Trip GetTripByName(string tripName)
        {
            return this.context.Trips
                .Include(x => x.Stops)
                .FirstOrDefault(x => x.Name == tripName);
        }

        public void AddStop(string tripName, Stop stop)
        {
            var trip = this.GetTripByName(tripName);

            if (trip == null)
            {
                return;
            }

            trip.Stops.Add(stop);
            this.context.Stops.Add(stop);
        }
    }
}