namespace TheWorld.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Models;

    using TheWorld.Services;

    using ViewModels;

    [Route("/api/trips/{tripname}/stops")]
    [Authorize]
    public class StopsController : Controller
    {
        private readonly IWorldRepository repository;

        private readonly ILogger<TripsController> logger;

        private readonly GeoCoordinatesService geoCoordinatesService;

        public StopsController(IWorldRepository repository, ILogger<TripsController> logger, GeoCoordinatesService geoCoordinatesService)
        {
            this.repository = repository;
            this.logger = logger;
            this.geoCoordinatesService = geoCoordinatesService;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = this.repository.GetUserTripByName(tripName, this.User.Identity.Name);
                return this.Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(x => x.Order).ToList()));
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to get stops: {0}", ex);
            }

            return this.BadRequest("Failed to get stops");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody] StopViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var stop = Mapper.Map<Stop>(viewModel);

                    var result = await this.geoCoordinatesService.GetCoordinatesAsync(stop.Name);

                    if (!result.Success)
                    {
                        this.logger.LogError(result.Message);
                    }
                    else
                    {
                        stop.Latitude = result.Latitude;
                        stop.Longitude = result.Longitude;
                    }

                    this.repository.AddStop(tripName, stop, this.User.Identity.Name);

                    if (await this.repository.SaveChangesAsync())
                    {
                        return this.Created($"/api/trips/{tripName}/{stop.Name}", Mapper.Map<StopViewModel>(stop));
                    }


                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Failed to save stops: {0}", ex);
            }

            return this.BadRequest("Failed to save stops");
        }

    }
}
