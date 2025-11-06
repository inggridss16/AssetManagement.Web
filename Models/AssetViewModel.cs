using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Web.Models
{
    public class AssetViewModel
    {
        public string Id { get; set; }
        public string AssetName { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
    }
}