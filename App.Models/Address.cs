using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Address
    {
        public Int32 Id { get; set; }
        [Required(ErrorMessage = "City is required")]
        [StringLength(45)]
        [DisplayName("City")]
        public string City { get; set; } 
        [Required(ErrorMessage = "Street Name is required")]
        [StringLength(45)]
        [DisplayName("Street Name")]
        public string Street { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [Required(ErrorMessage = "Street Number is required")]
        [DisplayName("Street Number")]
        public Int32 StreetNumber { get; set; }

        public Int32 RegionId { get; set; }
        public virtual Region Region { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
