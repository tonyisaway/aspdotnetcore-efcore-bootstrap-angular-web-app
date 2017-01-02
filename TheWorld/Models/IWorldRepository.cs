﻿namespace TheWorld.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWorldRepository
    {
        void AddStop(string tripName, Stop stop);

        void AddTrip(Trip trip);

        IEnumerable<Trip> GetAllTrips();

        Trip GetTripByName(string tripName);

        Task<bool> SaveChangesAsync();
    }
}