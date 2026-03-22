using JCT_Tracking_Api.Interface;
using JCT_Tracking_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JCT_Tracking_Api.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ContainerScheduleController : ControllerBase
    {
        private readonly IContainerRepository _containerService;

        public ContainerScheduleController(IContainerRepository containerService)
        {
            _containerService = containerService;
        }

        /// <summary>
        /// Get BL details with containers
        /// </summary>
        /// <param name="blNumber">Bill of Lading number</param>
        [HttpGet("fetch-bl")]
        public async Task<IActionResult> FetchBl([FromQuery] string blNumber)
        {
            if (string.IsNullOrWhiteSpace(blNumber))
                throw new ValidationException("BL number is required.");

            var blDetail = await _containerService.GetBlDetailAsync(blNumber);

            if (blDetail == null)
                throw new NotFoundException($"BL number '{blNumber}' not found.");

            return Ok(new
            {
                success = true,
                message = "BL details fetched successfully",
                data = blDetail
            });
        }

        /// <summary>
        /// Get container details by container number
        /// </summary>
        /// <param name="containerNumber">Container number</param>
        [HttpGet("fetch-container")]
        public async Task<IActionResult> FetchContainer([FromQuery] string containerNumber)
        {

            if (string.IsNullOrWhiteSpace(containerNumber))
                throw new ValidationException("Container number is required.");


            var container = await _containerService.GetContainerDetailAsync(containerNumber);

            if (container == null)
                throw new NotFoundException($"Container '{containerNumber}' not found.");

            return Ok(new
            {
                success = true,
                message = "Container details fetched successfully",
                data = container
            });
        }
    }
}
