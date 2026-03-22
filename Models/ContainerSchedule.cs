namespace JCT_Tracking_Api.Models
{
    public class ContainerSchedule
    {
    }

    public class BlDetail
    {
        public string BL { get; set; }
        public string VESSEL { get; set; }
        public string VOYAGE { get; set; }
        public string LINE { get; set; }
        public string CATEGORY { get; set; }
        public string CONSIGNEE { get; set; }
        public decimal TOTAL_CONTAINERS { get; set; }
        public string RELEASE_NBR { get; set; }
        public string SHIP_ID { get; set; }
        public string BL_KEY { get; set; }

        public virtual ICollection<ContainerDetail> Containers { get; set; }
    }

    public class ContainerDetail
    {
        public string CONTAINER_NBR { get; set; }
        public string EQ_SIZE { get; set; }
        public string CURRENT_STATUS { get; set; }
        public string BL_KEY { get; set; }

        public virtual BlDetail BLDetail { get; set; }
    }
}
