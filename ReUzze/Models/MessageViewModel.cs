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
    public class MessageViewModel
    {
        public Int32 UserID { get; set; }
        [DisplayName("To")]
        public string Email { get; set; }
        public string Subject { get; set; }
        [DisplayName("Send Copy to my Mail")]
        public bool Copy { get; set; }
        public decimal Bid { get; set; }
        [Required(ErrorMessage = "Body is required")]
        [DisplayName("Message")]
        [AllowHtml]
        public string Body { get; set; }
    }
     

}