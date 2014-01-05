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
    public class UserViewModel
    {
        public Int32 Id { get; set; }

        //public User User { get; set; }

        /* PERSON */
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(45)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "SurName is required")]
        [StringLength(255)]
        [DisplayName("SurName")]
        public string SurName { get; set; }

        /* ADDRESS */
        [Required(ErrorMessage = "City is required")]
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
        public virtual Region Region { get; set; }

        /* USER */
        [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10!")]
        [DisplayName("User")]
        public Int32 Rating { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(45)]
        [DisplayName("Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(45)]
        [DisplayName("Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [StringLength(255)]
        [DisplayName("Email")]
        public string Email { get; set; }

        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }

        /* REGION */
        public int SelectRegionId { get; set; }
        public SelectList Regions { get; set; }

        /* ROLE */
        public short SelectRoleId { get; set; }
        public SelectList Roles { get; set; }

        public string StatusMessage { get; set; }
    }
}