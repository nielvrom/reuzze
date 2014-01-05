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
    public class BidViewModel
    {
        public Int32 Id { get; set; }
        public Int64 EntityId { get; set; }
        public Int32 UserId { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        [DisplayName("Amount")]
        public Decimal Amount { get; set; }
        public DateTime Date { get; set; }
        
    }
     

}