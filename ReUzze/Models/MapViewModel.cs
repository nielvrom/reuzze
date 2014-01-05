using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using App.Models;

namespace ReUzze.Models
{
    public class MapViewModel
    {
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
    }
}