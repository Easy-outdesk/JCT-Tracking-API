using JCT_Tracking_Api.Interface;
using JCT_Tracking_Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<BlDetail> GetBlDetailAsync(string blNumber)
        {
            return await _context.BlDetails
                .Include(b => b.Containers)
                .FirstOrDefaultAsync(b => b.BL == blNumber);
        }

        public async Task<ContainerDetail> GetContainerDetailAsync(string containerNumber)
        {
            return await _context.ContainerDetails
                .Include(c => c.BLDetail)
                .FirstOrDefaultAsync(c => c.CONTAINER_NBR == containerNumber);
        }
    }
}
