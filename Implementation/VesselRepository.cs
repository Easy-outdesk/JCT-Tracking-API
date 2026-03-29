using JCT_Tracking_Api.DTO;
using JCT_Tracking_Api.Interface;
using JCT_Tracking_Api.Models;
using Microsoft.EntityFrameworkCore;
using Vessel_Tracking_Api.Enitity_Framework;
using Vessel_Tracking_Api.Models;

namespace JCT_Tracking_Api.Implementation
{
    public class VesselRepository :IVessselRepository
    {
        private readonly TrackingDbContext _context;

        public VesselRepository(TrackingDbContext context)
        {
            _context = context;
        }

        public async Task<List<VesselScheduleDto>> GetVesselSchedulesAsync(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ValidationException("From date cannot be later than To date.");

            var vessels = await _context.VesselSchedules
                .Where(v => v.EXPECTED_ARRIVAL >= from && v.EXPECTED_ARRIVAL <= to)
                .OrderBy(v => v.EXPECTED_ARRIVAL)
                .Select(v => new
                {
                    v.VESSEL_NAME,
                    v.VESSEL_CODE,
                    v.EXPECTED_ARRIVAL,
                    v.ACTUAL_ARRIVAL,
                    v.ACTUAL_DEPARTURE,
                    v.PHASE
                })
                .ToListAsync();

            if (!vessels.Any())
                throw new NotFoundException("No vessel schedules found for the given dates.");

            var result = vessels.Select(v => new VesselScheduleDto
            {
                VESSEL_NAME = v.VESSEL_NAME,
                VESSEL_CODE = v.VESSEL_CODE,
                EXPECTED_ARRIVAL = v.EXPECTED_ARRIVAL?.ToString("dd-MM-yyyy HH:mm"),
                ACTUAL_ARRIVAL = v.ACTUAL_ARRIVAL?.ToString("dd-MM-yyyy HH:mm"),
                ACTUAL_DEPARTURE = v.ACTUAL_DEPARTURE?.ToString("dd-MM-yyyy HH:mm"),
                PHASE = v.PHASE
            }).ToList();

            return result;
        }
    }
}
