using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetManagement.Web.Models
{
    public class AssetViewModel
    {
        public string? Id { get; set; }

        [Display(Name = "Asset Name")]
        [Required]
        public string AssetName { get; set; }

        [Required]
        public string Category { get; set; }

        [Display(Name = "Subcategory")]
        [Required]
        public string Subcategory { get; set; }

        public string? Status { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Responsible Person")]
        [Required]
        public long ResponsiblePersonId { get; set; }

        // This list will hold the options for the dropdown list.
        public List<SelectListItem>? CategoryOptions { get; set; }
        public List<SelectListItem>? SubcategoryOptions { get; set; }
        public List<SelectListItem>? ResponsiblePersonOptions { get; set; }

        // New property to hold maintenance records
        public IEnumerable<MaintenanceRecordViewModel>? MaintenanceRecords { get; set; }
    }
}