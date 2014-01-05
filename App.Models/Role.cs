using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Role
    {
        public Int16 Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        [StringLength(45)]
        [DisplayName("Role Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }

        //public virtual ICollection<User> Users { get; set; }
    }
}
