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
    public class LoginViewModel
    {
        // USERNAME
        [Required(ErrorMessage = "Username is required")]
        [StringLength(45)]
        [DisplayName("Username")]
        public string UserName { get; set; }
        // PASSWORD
        [Required(ErrorMessage = "Password is required")]
        [StringLength(45)]
        [DisplayName("Password")]
        [AllowHtml]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        // REMEMBER ME CHECKBOX
        [DisplayName("Remember me?")]
        public Boolean RememberMe { get; set; }
    }
}