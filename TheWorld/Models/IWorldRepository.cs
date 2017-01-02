namespace TheWorld.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWorldRepository
    {
        void AddStop(string tripName, Stop stop, string name);

        void AddTrip(Trip trip);

        IEnumerable<Trip> GetAllTrips();

        Trip GetTripByName(string tripName);

        IEnumerable<Trip> GetTripsByUsername(string name);

        Trip GetUserTripByName(string tripName, string userName);

        Task<bool> SaveChangesAsync();
    }
}