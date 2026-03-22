using JCT_Tracking_Api.Models;

namespace JCT_Tracking_Api.Interface
{
    public interface IContainerRepository
    {
        Task<BlDetail> GetBlDetailAsync(string blNumber);
        Task<ContainerDetail> GetContainerDetailAsync(string containerNumber);
    }
}
