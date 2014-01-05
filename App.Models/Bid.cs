using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Bid
    {
        public Int32 Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Id from User")]
        public Int32 UserId { get; set; }
        [Required]
        [DisplayName("Id from Entity")]
        public Int64 EntityId { get; set; }

        public virtual User User { get; set; }
        public virtual Entity Entity { get; set; }

        //public ICollection<Entity> Bids { get; set; }
    }
}
