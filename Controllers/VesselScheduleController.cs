using JCT_Tracking_Api.Interface;
using JCT_Tracking_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using Vessel_Tracking_Api.Enitity_Framework;


namespace Vessel_Tracking_Api.Controllers
{
    /// <summary>
    /// Controller to fetch vessel schedule information.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VesselScheduleController : ControllerBase
    {
        private readonly TrackingDbContext _context;
        private readonly IVessselRepository _vesselService;

        public VesselScheduleController(TrackingDbContext context, IVessselRepository vesselService)
        {
            _context = context;
            _vesselService = vesselService;
        }

        [HttpGet("FetchJctVessel")]
        [EnableRateLimiting("ApiPolicy")]
        public async Task<IActionResult> Fetch([FromQuery] string fromdate, [FromQuery] string todate)
        {
            if (string.IsNullOrWhiteSpace(fromdate) || string.IsNullOrWhiteSpace(todate))
                throw new ValidationException("Both fromdate and todate are required.");

            if (!DateTime.TryParse(fromdate, out var fromParsed) ||
                !DateTime.TryParse(todate, out var toParsed))
            {
                throw new ValidationException("Invalid date format. Use yyyy-MM-dd or yyyy-MM-ddTHH:mm.");
            }

            var from = fromParsed.Date.AddHours(6).AddSeconds(1);
            var to = toParsed.Date.AddHours(6);

            var vesselSchedules = await _vesselService.GetVesselSchedulesAsync(from, to);


            if (vesselSchedules == null || !vesselSchedules.Any())
                throw new NotFoundException("No vessel schedules found for the given dates.");

            return Ok(new
            {
                success = true,
                message = "Vessel schedules fetched successfully",
                data = vesselSchedules
            });
        }

    }
}

