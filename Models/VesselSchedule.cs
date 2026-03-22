using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vessel_Tracking_Api.Models
{

    [Keyless]
    [Table("NCT_VIEW_WEB_VESSEL_INFO", Schema = "tos_usr")]
    public class VesselSchedule
    {
        public string VESSEL_NAME { get; set; }
        public string VESSEL_CODE { get; set; }
        public string VOYAGE_NUMBER { get; set; }
        public string VESSEL_LINE { get; set; }
        public string AGENT_NAME { get; set; }
        public string VESSEL_CLASS { get; set; }
        public decimal? VESSEL_LENGTH { get; set; }
        public string PORT_ROTATION { get; set; }
        public string CUSTOMS_ROTATION { get; set; }
        public string BERTH { get; set; }

        // Use DateTime for proper LINQ filtering
        public DateTime? EXPECTED_ARRIVAL { get; set; }
        public DateTime? EXPECTED_DEPARTURE { get; set; }
        public DateTime? ACTUAL_ARRIVAL { get; set; }
        public DateTime? ACTUAL_DEPARTURE { get; set; }

        public string PHASE { get; set; }
    }
}
