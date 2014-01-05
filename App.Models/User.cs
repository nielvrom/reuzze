using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class User
    {
        public Int32 Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(45)]
        [DisplayName("Username")]
        public string UserName { get; set; }

        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(255)]
        [DisplayName("Email")]
        public string Email { get; set; }
        public Int32 Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public Nullable<DateTime> LockedDate { get; set; }
        public Nullable<DateTime> LastLoggedInDate { get; set; }

        public Int32 PersonId { get; set; }
        public Int16 RoleId { get; set; }

        public virtual Person Person { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Entity> Entities { get; set; }
        public virtual ICollection<Entity> Favorites { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }

        //public virtual ICollection<Role> Roles { get; set; }
    }
}
