namespace TheWorld.Models
{
    using System;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class WorldUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}