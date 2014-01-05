using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Entity
    {
        public Int64 Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255)]
        [DisplayName("Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [DisplayName("Description")]
        public string Description { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        /*[Required(ErrorMessage = "Type is required")]
        [StringLength(16)]
        [DisplayName("Type")]
        public string Type { get; set; }*/
        [Required]
        public decimal InstantSellingPrice { get; set; }
        public Nullable<decimal> ShippingPrice { get; set; }

        public Condition? Condition { get; set; }
        public Nullable<Int64> Views { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }

        public Int32 UserId { get; set; }

        public Int32 RegionId { get; set; }

        public Int16 CategoryId { get; set; }

        public virtual User User { get; set; }
        public virtual Region Region { get; set; }
        public virtual Category Category { get; set; }
        //public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<User> Favorites { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Media> Media { get; set; }
    }

    public enum Condition
    {
        New = 1,
        Used = 2
    }
}
