//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReUzze.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class entity
    {
        public entity()
        {
            this.bids = new HashSet<bid>();
            this.favorites = new HashSet<favorite>();
            this.media = new HashSet<medium>();
        }
    
        public long entity_id { get; set; }
        public string entity_title { get; set; }
        public string entity_description { get; set; }
        public System.DateTime entity_starttime { get; set; }
        public System.DateTime entity_endtime { get; set; }
        public decimal entity_instantsellingprice { get; set; }
        public Nullable<decimal> entity_shippingprice { get; set; }
        public string entity_condition { get; set; }
        public Nullable<long> entity_views { get; set; }
        public System.DateTime entity_created { get; set; }
        public Nullable<System.DateTime> entity_modified { get; set; }
        public Nullable<System.DateTime> entity_deleted { get; set; }
        public int user_id { get; set; }
        public int region_id { get; set; }
        public sbyte category_id { get; set; }
    
        public virtual ICollection<bid> bids { get; set; }
        public virtual category category { get; set; }
        public virtual region region { get; set; }
        public virtual user user { get; set; }
        public virtual ICollection<favorite> favorites { get; set; }
        public virtual ICollection<medium> media { get; set; }
    }
}
