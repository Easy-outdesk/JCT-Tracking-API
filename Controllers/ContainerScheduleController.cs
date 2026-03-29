using JCT_Tracking_Api.DTO;
using JCT_Tracking_Api.Interface;
using JCT_Tracking_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Data;

namespace JCT_Tracking_Api.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ContainerScheduleController : ControllerBase
    {
        private readonly IContainerRepository _containerService;
        private readonly ILogger<ContainerSchedule> _logger;

        public ContainerScheduleController(IContainerRepository containerService, ILogger<ContainerSchedule> logger)
        {
            _containerService = containerService;
            _logger = logger;
        }

        /// <summary>
        /// Get BL details with containers
        /// </summary>
        /// <param name="blNumber">Bill of Lading number</param>
        [HttpPost("shipment-tracking")]
        [EnableRateLimiting("ApiPolicy")]
        public async Task<IActionResult> FetchBl([FromBody] ShipmentTrackingRequest request)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var requestedUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}";

            string? blNumber = request?.BlNumber;
            string? containerNumber = request?.ContainerNumber;

            _logger.LogInformation("API Hit: FetchBl | BL: {BL} | Container: {Container} | From IP: {IP} | User-Agent: {UA} | URL: {URL}",
                blNumber, containerNumber, ipAddress, userAgent, requestedUrl);

            bool hasBlNumber = !string.IsNullOrWhiteSpace(blNumber);
            bool hasContainerNumber = !string.IsNullOrWhiteSpace(containerNumber);

            // Validation
            if (!hasBlNumber)
                return BadRequest(new { success = false, message = "BL number is required." });

            if (hasContainerNumber)
            {
                var containerList = await _containerService.GetBlContainersAsync(blNumber!, containerNumber!);

                if (containerList == null || !containerList.Any())
                {
                    _logger.LogWarning("Container '{Container}' for BL '{BL}' not found. Requested from IP {IP}", containerNumber, blNumber, ipAddress);
                    return NotFound(new { success = false, message = $"Container '{containerNumber}' for BL '{blNumber}' not found." });
                }

                return Ok(new
                {
                    success = true,
                    message = $"Container '{containerNumber}' for BL '{blNumber}' fetched successfully",
                    data = containerList
                });
            }
            else
            {
                var blDetail = await _containerService.GetBlDetailAsync(blNumber!);

                if (blDetail == null)
                {
                    _logger.LogWarning("BL '{BL}' not found. Requested from IP {IP}", blNumber, ipAddress);
                    return NotFound(new { success = false, message = $"BL '{blNumber}' not found." });
                }

                return Ok(new
                {
                    success = true,
                    message = $"BL '{blNumber}' details fetched successfully",
                    data = blDetail
                });
            }
        }

        /// <summary>
        /// Get container details by container number
        /// </summary>
        /// <param name="containerNumber">Container number</param>
       // [HttpGet("fetch-containers")]
        //public async Task<IActionResult> FetchContainer([FromQuery] string containerNumber)
        //{

        //    if (string.IsNullOrWhiteSpace(containerNumber))
        //        throw new ValidationException("Container number is required.");


        //    var container = await _containerService.GetContainerDetailAsync(containerNumber);

        //    if (container == null)
        //        throw new NotFoundException($"Container '{containerNumber}' not found.");

        //    return Ok(new
        //    {
        //        success = true,
        //        message = "Container details fetched successfully",
        //        data = container
        //    });
        //}
    }

}
