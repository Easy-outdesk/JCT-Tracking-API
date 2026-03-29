using JCT_Tracking_Api.DTO;
using JCT_Tracking_Api.Models;

namespace JCT_Tracking_Api.Interface
{
    public interface IContainerRepository
    {
        Task<BlContainerResponseDto> GetBlDetailAsync(string blNumber);
        Task<List<ContainerDetailsDto>> GetBlContainersAsync(string blNumber, string containerNumber);
    }
}
