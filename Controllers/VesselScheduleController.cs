using JCT_Tracking_Api.Interface;
using JCT_Tracking_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Globalization;
using Vessel_Tracking_Api.Enitity_Framework;
using Vessel_Tracking_Api.Models;


namespace Vessel_Tracking_Api.Controllers
{
    /// <summary>
    /// Controller to fetch vessel schedule information.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class VesselScheduleController : ControllerBase
    {
        private readonly TrackingDbContext _context;
        private readonly IVessselRepository _vesselService;
        ILogger<VesselSchedule> _logger;

        public VesselScheduleController(TrackingDbContext context, IVessselRepository vesselService, ILogger<VesselSchedule> logger)
        {
            _context = context;
            _vesselService = vesselService;
            _logger = logger;
        }

        [HttpPost("tracking-by-vessel")]
        [EnableRateLimiting("ApiPolicy")]
        public async Task<IActionResult> Fetch([FromBody] VesselTrackingRequest request)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var requestedUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";

            string? fromdate = request?.FromDate;
            string? todate = request?.ToDate;

            _logger.LogInformation("TrackingByVessel request received. IP: {IP}, User-Agent: {UA}, URL: {URL}, FromDate: {FromDate}, ToDate: {ToDate}",
                ipAddress, userAgent, requestedUrl, fromdate, todate);

            // Fixed times
            var fromTime = TimeSpan.Parse("06:00:01");
            var toTime = TimeSpan.Parse("06:00:00");

            DateTime fromParsed;
            DateTime toParsed;

            if (string.IsNullOrWhiteSpace(fromdate) && string.IsNullOrWhiteSpace(todate))
            {
                fromParsed = DateTime.Today.AddDays(-3).Add(fromTime);
                toParsed = DateTime.Today.AddDays(10).Add(toTime);
            }
            else if (!string.IsNullOrWhiteSpace(fromdate) && string.IsNullOrWhiteSpace(todate))
            {
                throw new ValidationException("To date is required when From date is provided.");
            }
            else if (string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                throw new ValidationException("From date is required when To date is provided.");
            }
            else
            {
                string[] allowedFormats = { "dd-MM-yyyy" };

                if (!DateTime.TryParseExact(fromdate, allowedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out fromParsed))
                {
                    throw new ValidationException($"Invalid From date format: '{fromdate}'. Use: dd-MM-yyyy.");
                }

                if (!DateTime.TryParseExact(todate, allowedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out toParsed))
                {
                    throw new ValidationException($"Invalid To date format: '{todate}'. Use: dd-MM-yyyy.");
                }

                fromParsed = fromParsed.Date.Add(fromTime);
                toParsed = toParsed.Date.Add(toTime);

                if (fromParsed > toParsed)
                    throw new ValidationException("From date cannot be later than To date.");
            }

            var vesselSchedules = await _vesselService.GetVesselSchedulesAsync(fromParsed, toParsed);

            _logger.LogInformation("Vessel schedule '{fromDate}' To date '{todate}' fetched successfully from IP {IP}", fromParsed, toParsed, ipAddress);

            return Ok(new
            {
                success = true,
                message = "Vessel schedules fetched successfully",
                data = vesselSchedules
            });
        }

    }
}

