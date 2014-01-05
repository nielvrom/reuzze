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
    public class ChangeSettingsViewModel
    {
        /* PERSON */
        /*[Required(ErrorMessage = "First Name is required")]
        [StringLength(45)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "SurName is required")]
        [StringLength(255)]
        [DisplayName("SurName")]
        public string SurName { get; set; }*/

        /* ADDRESS */
        /*[Required(ErrorMessage = "City is required")]
        [StringLength(45)]
        [DisplayName("City")]
        public string City { get; set; }
        [Required(ErrorMessage = "Street Name is required")]
        [StringLength(45)]
        [DisplayName("Street Name")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Street Number is required")]
        [DisplayName("Street Number")]
        public Int32 StreetNumber { get; set; }
        public virtual Region Region { get; set; }*/

        /* USER */
        [Required(ErrorMessage = "Username is required")]
        [StringLength(45)]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }  

        /*[Required(ErrorMessage = "Email is required")]
        [StringLength(255)]
        [DisplayName("Email")]
        public string Email { get; set; }*/

        /* REGION */
        /*public int SelectRegionId { get; set; }
        public SelectList Regions { get; set; }*/
    }
}