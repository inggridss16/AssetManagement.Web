using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Web.Models
{
    public class ApprovalSubmitViewModel
    {
        [Required]
        public string AssetId { get; set; }

        public string? Comments { get; set; }

        [Required]
        public string Action { get; set; }
    }
}