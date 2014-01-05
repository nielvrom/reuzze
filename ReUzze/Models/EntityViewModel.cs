using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using App.Models;

namespace ReUzze.Models
{
    public class EntityViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255)]
        [DisplayName("Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [DisplayName("Description")]
        public string Description { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        /*[Required(ErrorMessage = "Type is required")]
        [StringLength(16)]
        [DisplayName("Type")]*/
        //public string Type { get; set; }

        [Required]
        [DisplayName("Instant selling price (EUR)")]
        public Decimal InstantSellingPrice { get; set; }
        public Nullable<Decimal> ShippingPrice { get; set; }
        public Int64 Views { get; set; }

        /* USER */
        public Int32 UserId { get; set; }

        /* REGION */
        public int RegionId { get; set; }

        /* CATEGORY */
        public short? SelectedCategoryId { get; set; }
        public SelectList Categories { get; set; }
        public IEnumerable<ReUzze.Helpers.GroupedSelectListItem> GroupedTypeOptions { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        //public int ConditionId { get; set; }

        public Condition? Condition { get; set; }

        public string StatusMessage { get; set; }
    }
     
    public enum Condition
    {
        New=1,
        Used=2
    }

}