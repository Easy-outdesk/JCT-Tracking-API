using JCT_Tracking_Api.DTO;
using JCT_Tracking_Api.Interface;
using JCT_Tracking_Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using Vessel_Tracking_Api.Enitity_Framework;

namespace JCT_Tracking_Api.Implementation
{
    public class ContainerRepository : IContainerRepository
    {

        private readonly TrackingDbContext _context;

        public ContainerRepository(TrackingDbContext context)
        {
            _context = context;
        }
        public async Task<BlContainerResponseDto> GetBlDetailAsync(string blNumber)
        {
            if (string.IsNullOrWhiteSpace(blNumber))
                throw new ValidationException("BL number is required.");

            blNumber = blNumber.Trim().ToUpper();

            // Fetch BL Details
            var blDetails = await _context.BlDetails
                .AsNoTracking()
                .Where(b => b.BL == blNumber)
                .Select(b => new BlDetailDto
                {
                    BL = b.BL,
                    VOYAGE = b.VOYAGE,
                    LINE = b.LINE,
                    CATEGORY = b.CATEGORY,
                    SHIP_ID = b.SHIP_ID,
                    BL_KEY = b.BL_KEY
                })
                .ToListAsync();

            if (!blDetails.Any())
                throw new NotFoundException($"No BL found for '{blNumber}'");

            // Extract BL_KEYS
            var blKeys = blDetails.Select(x => x.BL_KEY).Distinct().ToList();

            // Fetch Containers
            var containers = await _context.ContainerDetails
          .AsNoTracking()
          .Where(c => blKeys.Contains(c.BL_NBR))
          .Select(c => new
          {
              c.CONTAINER_NUMBER,
              c.BL_NBR,
              c.SIZE,
              c.TYPE,
              c.HEIGHT,
              c.LOCATION
          })
          .ToListAsync();
            var containerDetails = containers .Select(c => new ContainerDetailDto
                {
                    CONTAINER_NUMBER = c.CONTAINER_NUMBER,
                    BL_NBR = c.BL_NBR,
                    EQ_SIZE =
                        (c.SIZE.HasValue ? c.SIZE.Value.ToString("0") : "") + "/" +
                        (c.TYPE ?? "") + "/" +
                        (c.HEIGHT.HasValue ? c.HEIGHT.Value.ToString("0") : ""),
                    CURRENT_STATUS = c.LOCATION
                }).ToList();

            // Final Response
            var response = new BlContainerResponseDto
            {
                BL_NO = blNumber,
                Bl_Details = blDetails,
                Containers = containerDetails
            };

            return response;
        }

        public async Task<List<ContainerDetailsDto>> GetBlContainersAsync(string blNumber, string containerNumber)
        {
            if (string.IsNullOrWhiteSpace(blNumber))
                throw new ValidationException("BL number is required.");

            if (string.IsNullOrWhiteSpace(containerNumber))
                throw new ValidationException("Container number is required.");

            blNumber = blNumber.Trim().ToUpper();
            containerNumber = containerNumber.Trim().ToUpper();

            var containers = await _context.ContainerDetails
                .AsNoTracking()
                .Where(c => c.BL_NBR == blNumber && c.CONTAINER_NUMBER == containerNumber)
                .Select(c => new
                {
                    c.CONTAINER_NUMBER,
                    c.BL_NBR,
                    c.TYPE,
                    c.TARE_WEIGHT,
                    c.SIZE,
                    c.LINE,
                    c.ISO_CODE,
                    c.GROSS_WEIGHT,
                    c.LOCATION,
                    c.HEIGHT,
                    c.DAMAGE,
                    c.LOAD_PORT,
                    c.ORIGIN,
                    c.GROUP,
                    c.DISCHARGE_PORT,
                    c.DESTINATION,
                    c.DISCHARGE_PORT_OPTIONAL,
                    c.CATEGORY,
                    c.SEALS,
                    c.STATUS,
                    c.ACTUAL_ARRIVAL_TIME,
                    c.ACTUAL_DEPATURE_TIME,
                    c.BAYAN_NO,
                    c.RELEASE_STAT,
                    c.ARRIVAL,
                    c.DEPARTURE
                })
                .ToListAsync();

            if (!containers.Any())
            {
                var blExists = await _context.ContainerDetails
                     .AsNoTracking()
                     .Where(c => c.BL_NBR == blNumber)
                     .Select(c => 1)
                     .FirstOrDefaultAsync() != 0;

                if (!blExists)
                    throw new NotFoundException($"BL '{blNumber}' not found.");

                throw new NotFoundException($"Container '{containerNumber}' for BL '{blNumber}' not found.");
            }

            var result = containers.Select(c => new ContainerDetailsDto
            {
                CONTAINER_NUMBER = c.CONTAINER_NUMBER,
                BL_NBR = c.BL_NBR,
                TYPE = c.TYPE,
                TARE_WEIGHT = c.TARE_WEIGHT,
                SIZE = c.SIZE,
                LINE = c.LINE,
                ISO_CODE = c.ISO_CODE,
                GROSS_WEIGHT = c.GROSS_WEIGHT,
                LOCATION = c.LOCATION,
                HEIGHT = c.HEIGHT,
                DAMAGE = c.DAMAGE,
                LOAD_PORT = c.LOAD_PORT,
                ORIGIN = c.ORIGIN,
                GROUP = c.GROUP,
                DISCHARGE_PORT = c.DISCHARGE_PORT,
                DESTINATION = c.DESTINATION,
                DISCHARGE_PORT_OPTIONAL = c.DISCHARGE_PORT_OPTIONAL,
                CATEGORY = c.CATEGORY,
                SEALS = c.SEALS,
                STATUS = c.STATUS,
                ACTUAL_ARRIVAL_TIME = c.ACTUAL_ARRIVAL_TIME?.ToString("dd-MM-yyyy HH:mm"),
                ACTUAL_DEPATURE_TIME = c.ACTUAL_DEPATURE_TIME?.ToString("dd-MM-yyyy HH:mm"),
                BAYAN_NO = c.BAYAN_NO,
                EXPORT_PERMIT_STATUS = c.RELEASE_STAT,
                ARRIVAL = c.ARRIVAL,
                DEPARTURE = c.DEPARTURE
            }).ToList();

            return result;
        }
    }
}
