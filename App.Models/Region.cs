using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Region
    {
        public Int32 Id { get; set; }
        [Required(ErrorMessage = "Region Name is required")]
        [StringLength(45)]
        [DisplayName("Region Name")]
        public string Name { get; set; }

        public virtual ICollection<Entity> Entities { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
