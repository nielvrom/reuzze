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
    
    public partial class category
    {
        public category()
        {
            this.categories1 = new HashSet<category>();
            this.entities = new HashSet<entity>();
        }
    
        public sbyte category_id { get; set; }
        public string category_name { get; set; }
        public string category_description { get; set; }
        public System.DateTime category_created { get; set; }
        public Nullable<System.DateTime> category_modified { get; set; }
        public Nullable<System.DateTime> category_deleted { get; set; }
        public Nullable<sbyte> category_parentid { get; set; }
    
        public virtual ICollection<category> categories1 { get; set; }
        public virtual category category1 { get; set; }
        public virtual ICollection<entity> entities { get; set; }
    }
}
