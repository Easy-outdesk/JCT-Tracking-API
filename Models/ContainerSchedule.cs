using System.ComponentModel.DataAnnotations.Schema;

namespace JCT_Tracking_Api.Models
{
    public class ContainerSchedule
    {
    }

    public class ShipmentTrackingRequest
    {
        public string? BlNumber { get; set; }
        public string? ContainerNumber { get; set; }
    }

    public class VesselTrackingRequest
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }

    public class BlDetail
    {
        public string? BL { get; set; }
        public string? VESSEL { get; set; }
        public string? VOYAGE { get; set; }
        public string? LINE { get; set; }
        public string? CATEGORY { get; set; }
        public string? CONSIGNEE { get; set; }
        public decimal? TOTAL_CONTAINERS { get; set; }   // nullable
        public string? RELEASE_NBR { get; set; }
        public string? SHIP_ID { get; set; }
        public string? BL_KEY { get; set; }

        public virtual ICollection<ContainerDetail> Containers { get; set; }
    }

    public class ContainerDetail
    {
        public string CONTAINER_NUMBER { get; set; } = null!;
        public string? BL_NBR { get; set; }
        public string? ARRIV_LOCATION { get; set; }
        public string? TYPE { get; set; }
        public decimal? TARE_WEIGHT { get; set; }
        public decimal? SIZE { get; set; }
        public string? POSITION { get; set; }
        public string? LINE { get; set; }
        public string? STATE { get; set; }
        public string? ISO_CODE { get; set; }
        public decimal? GROSS_WEIGHT { get; set; }
        public string? LOCATION { get; set; }
        public decimal? HEIGHT { get; set; }
        public string? DAMAGE { get; set; }
        public string? LOAD_PORT { get; set; }
        public string? ORIGIN { get; set; }
        public string? GROUP { get; set; }
        public string? DISCHARGE_PORT { get; set; }
        public string? DESTINATION { get; set; }
        public string? DISCHARGE_PORT_OPTIONAL { get; set; }
        public string? COMMODITY { get; set; }
        public string? SHIPPER { get; set; }
        public string? CONSIGNEE { get; set; }
        public string? CATEGORY { get; set; }
        public string? HAZARDS_CLASS { get; set; }
        public decimal? TEMPERATURE { get; set; }
        public string? SEALS { get; set; }
        public string? REMARKS { get; set; }
        public string? STATUS { get; set; }
        public string? INTENDED_ARRIVAL_LOCATION { get; set; }
        public string? INTENDED_ARRIVAL_POSITION { get; set; }
        public string? INTENDED_ARRIVAL_VOYAGE { get; set; }
        public string? INTENDED_ARRIVAL_TIME { get; set; }
        public string? INTENDED_DEPATURE_LOCATION { get; set; }
        public string? INTENDED_DEPATURE_POSITION { get; set; }
        public string? INTENDED_DEPARTURE_VOYAGE { get; set; }
        public string? INTENDED_DEPATURE_TIME { get; set; }
        public string? ACTUAL_ARRIVAL_LOCATION { get; set; }
        public string? ACTUAL_ARRIVAL_POSITION { get; set; }
        public string? ACTUAL_ARRIVAL_VOYAGE { get; set; }
        public DateTime? ACTUAL_ARRIVAL_TIME { get; set; }
        public string? ACTUAL_DEPATURE_LOCATION { get; set; }
        public string? ACTUAL_DEPATURE_POSITION { get; set; }
        public string? ACTUAL_DEPATURE_VOYAGE { get; set; }
        public DateTime? ACTUAL_DEPATURE_TIME { get; set; }
        public string? BAYAN_NO { get; set; }
        public string? RELEASE_STAT { get; set; }
        public string? ARRIVAL { get; set; }
        public string? DEPARTURE { get; set; }

        public virtual BlDetail BLDetail { get; set; } = null!;
    }
}
