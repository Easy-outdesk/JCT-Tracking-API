using JCT_Tracking_Api.Models;

namespace JCT_Tracking_Api.DTO
{
    public class DTO
    {
    }

    public class BlContainerResponseDto
    {
        public string BL_NO { get; set; }
        public List<BlDetailDto> Bl_Details { get; set; }
        public List<ContainerDetailDto> Containers { get; set; }
    }

    public class BlDetailDto
    {
        public string? BL { get; set; }
        //public string? VESSEL { get; set; }
        public string? VOYAGE { get; set; }
        public string LINE { get; set; }
        public string? CATEGORY { get; set; }
        //public string? CONSIGNEE { get; set; }
        //public decimal? TOTAL_CONTAINERS { get; set; }
        //public string? RELEASE_NBR { get; set; }
        public string? SHIP_ID { get; set; }
        public string BL_KEY { get; set; }
    }

    public class ContainerDetailDto
    {
        public string CONTAINER_NUMBER { get; set; } = null!;
        public string? BL_NBR { get; set; }
        public string? EQ_SIZE { get; set; }
        public string? CURRENT_STATUS { get; set; }

        //public virtual BlDetail BLDetail { get; set; } = null!;
    }

    public class ContainerDetailsDto
    {
        public string CONTAINER_NUMBER { get; set; } = null!;
        public string? BL_NBR { get; set; }
        //public string? ARRIV_LOCATION { get; set; }
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
        //public string? COMMODITY { get; set; }
        //public string? SHIPPER { get; set; }
        //public string? CONSIGNEE { get; set; }
        public string? CATEGORY { get; set; }
        //public string? HAZARDS_CLASS { get; set; }
        //public decimal? TEMPERATURE { get; set; }
        public string? SEALS { get; set; }
        //public string? REMARKS { get; set; }
        public string? STATUS { get; set; }
        //public string? INTENDED_ARRIVAL_LOCATION { get; set; }
        //public string? INTENDED_ARRIVAL_POSITION { get; set; }
        //public string? INTENDED_ARRIVAL_VOYAGE { get; set; }
        //public string? INTENDED_ARRIVAL_TIME { get; set; }
        //public string? INTENDED_DEPATURE_LOCATION { get; set; }
        //public string? INTENDED_DEPATURE_POSITION { get; set; }
        //public string? INTENDED_DEPARTURE_VOYAGE { get; set; }
        //public string? INTENDED_DEPATURE_TIME { get; set; }
        //public string? ACTUAL_ARRIVAL_LOCATION { get; set; }
        //public string? ACTUAL_ARRIVAL_POSITION { get; set; }
        //public string? ACTUAL_ARRIVAL_VOYAGE { get; set; }
        public string? ACTUAL_ARRIVAL_TIME { get; set; }
        //public string? ACTUAL_DEPATURE_LOCATION { get; set; }
        //public string? ACTUAL_DEPATURE_POSITION { get; set; }
        //public string? ACTUAL_DEPATURE_VOYAGE { get; set; }
        public string? ACTUAL_DEPATURE_TIME { get; set; }
        public string? BAYAN_NO { get; set; }
        public string? EXPORT_PERMIT_STATUS { get; set; }
        public string? ARRIVAL { get; set; }
        public string? DEPARTURE { get; set; }
    }


    public class VesselScheduleDto
    {
        public string? VESSEL_NAME { get; set; }
        public string? VESSEL_CODE { get; set; }
        //public string? VOYAGE_NUMBER { get; set; }
        //public string? VESSEL_LINE { get; set; }
        //public string? AGENT_NAME { get; set; }
        //public string? VESSEL_CLASS { get; set; }
        //public decimal? VESSEL_LENGTH { get; set; }
        //public string? PORT_ROTATION { get; set; }
        //public string? CUSTOMS_ROTATION { get; set; }
        //public string? BERTH { get; set; }

        // Use DateTime for proper LINQ filtering
        public string? EXPECTED_ARRIVAL { get; set; }
        //public DateTime? EXPECTED_DEPARTURE { get; set; }
        public string? ACTUAL_ARRIVAL { get; set; }
        public string? ACTUAL_DEPARTURE { get; set; }

        public string PHASE { get; set; }
    }
}
