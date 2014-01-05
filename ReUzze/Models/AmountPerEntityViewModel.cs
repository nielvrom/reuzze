using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using App.Models;

namespace ReUzze.Models
{
    public class AmountPerEntityViewModel
    {
        public int AmountEntities { get; set; }
        public int AmountCategories { get; set; }
        public int AmountUsers { get; set; }
        public int AmountRoles { get; set; }
        public int AmountAddresses { get; set; }
        public int AmountRegions { get; set; }
        public int AmountImages { get; set; }
        public int AmountBids { get; set; }
    }
}