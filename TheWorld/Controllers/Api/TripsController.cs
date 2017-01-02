namespace TheWorld.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Models;
    using ViewModels;

    [Route("api/Trips")]
    public class TripsController : Controller
    {
        private IWorldRepository repository;

        private ILogger<TripsController> logger;

        public TripsController(IWorldRepository repository,
            ILogger<TripsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            //if (true)
            //{
            //    return this.BadRequest("Bad things happen");
            //}

            try
            {
                var results = this.repository.GetAllTrips();
                return this.Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get All Trips: {ex}");
            }

            return this.BadRequest("Error occurred");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var trip = Mapper.Map<Trip>(viewModel);

                this.repository.AddTrip(trip);

                if (await this.repository.SaveChangesAsync())
                { 
                    this.Created($"api/trips/{ trip.Name }", Mapper.Map<TripViewModel>(trip));
                }
                else
                {
                    return this.BadRequest("Failed to save changes to the ");
                }
            }

            return this.BadRequest("Failed to save the trip");
        }

    }
}
