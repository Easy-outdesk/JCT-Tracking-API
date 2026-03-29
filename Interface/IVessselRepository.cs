using JCT_Tracking_Api.DTO;
using Vessel_Tracking_Api.Models;

namespace JCT_Tracking_Api.Interface
{
    public interface IVessselRepository
    {
        Task<List<VesselScheduleDto>> GetVesselSchedulesAsync(DateTime from, DateTime to);
    }
}
