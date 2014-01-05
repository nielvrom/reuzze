using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using App.Models;

namespace ReUzze.Models
{
    public class AddressViewModel
    {
        public Address Address { get; set; }

        public int SelectRegionId { get; set; }
        public SelectList Regions { get; set; }

        public string StatusMessage { get; set; }
    }
}