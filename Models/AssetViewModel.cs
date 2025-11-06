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

        public string? Status { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Subcategory")]
        [Required]
        public string Subcategory { get; set; }


        // This list will hold the options for the 'Category' dropdown list.
        public List<SelectListItem>? CategoryOptions { get; set; }

        // This list will hold the options for the dropdown list.
        public List<SelectListItem>? SubcategoryOptions { get; set; }
    }
}