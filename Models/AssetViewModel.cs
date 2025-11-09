using System;
using System.Collections.Generic;
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

        public long RequesterId { get; set; }

        [Display(Name = "Requester")]
        public string? RequesterName { get; set; }

        [Display(Name = "Responsible Person")]
        [Required]
        public long ResponsiblePersonId { get; set; }

        public List<SelectListItem>? CategoryOptions { get; set; }
        public List<SelectListItem>? SubcategoryOptions { get; set; }
        public List<SelectListItem>? ResponsiblePersonOptions { get; set; }

        public IEnumerable<MaintenanceRecordViewModel>? MaintenanceRecords { get; set; }

        // Add this property for the approval logs
        public IEnumerable<TrxAssetApprovalViewModel>? ApprovalLogs { get; set; }
    }
}