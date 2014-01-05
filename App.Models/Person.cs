using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Models
{
    public class Person
    {
        public Int32 Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(45)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "SurName is required")]
        [StringLength(255)]
        [DisplayName("SurName")]
        public string SurName { get; set; }
        
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }

        public Int32 AddressId { get; set; }

        public virtual Address Address { get; set; }
    }
}
