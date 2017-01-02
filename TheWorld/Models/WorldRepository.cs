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

        public void AddStop(string tripName, Stop stop, string userName)
        {
            var trip = this.GetUserTripByName(tripName, userName);

            if (trip == null)
            {
                return;
            }

            trip.Stops.Add(stop);
            this.context.Stops.Add(stop);
        }

        public void AddTrip(Trip trip)
        {
            this.context.Trips.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            this.logger.LogInformation("Getting All Trips from the Database");
            return this.context.Trips.ToList();
        }

        public Trip GetTripByName(string tripName)
        {
            return this.context.Trips.Include(x => x.Stops).FirstOrDefault(x => x.Name == tripName);
        }

        public IEnumerable<Trip> GetTripsByUsername(string name)
        {
            this.logger.LogInformation("Getting All Trips from the Database");
            return this.context.Trips.Include(x => x.Stops).Where(x => x.UserName == name).ToList();
        }

        public Trip GetUserTripByName(string tripName, string userName)
        {
            return
                this.context.Trips.Include(x => x.Stops)
                    .FirstOrDefault(x => x.UserName == userName && x.Name == tripName);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await this.context.SaveChangesAsync()) > 0;
        }
    }
}