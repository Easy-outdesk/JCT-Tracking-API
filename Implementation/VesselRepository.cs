using JCT_Tracking_Api.Interface;
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

        public async Task<List<VesselSchedule>> GetVesselSchedulesAsync(DateTime from, DateTime to)
        {
            return await _context.VesselSchedules
                .Where(v => v.EXPECTED_ARRIVAL >= from && v.EXPECTED_ARRIVAL <= to)
                .OrderBy(v => v.EXPECTED_ARRIVAL)
                .ToListAsync();
        }
    }
}
