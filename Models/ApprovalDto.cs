namespace AssetManagement.Web.Models
{
    public class ApprovalDto
    {
        public string AssetId { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }
}