namespace AssetManagement.Web.Models
{
    public class MaintenanceRecordViewModel
    {
        public long Id { get; set; }
        public string LinkedAssetId { get; set; }
        public decimal MaintenanceCost { get; set; }
        public string MaintenanceType { get; set; }
        public string Comments { get; set; }
        public string Vendor { get; set; }
        public DateTime MaintenanceDate { get; set; }
    }
}