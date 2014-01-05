using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Media
    {
        public Int32 Id { get; set; }
        public string URL { get; set; }
        public string MediaType { get; set; }
        public string MimeType { get; set; }
        public Nullable<Boolean> IsExternal { get; set; }

        public Int64 EntityId { get; set; }
        public virtual Entity Entity { get; set; }
    }
}
