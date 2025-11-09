using System;

namespace AssetManagement.Web.Models
{
    public class TrxAssetApprovalViewModel
    {
        public string ApproverName { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}