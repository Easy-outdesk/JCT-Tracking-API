using Vessel_Tracking_Api.Models;

namespace JCT_Tracking_Api.Interface
{
    public interface IVessselRepository
    {
        Task<List<VesselSchedule>> GetVesselSchedulesAsync(DateTime from, DateTime to);
    }
}
