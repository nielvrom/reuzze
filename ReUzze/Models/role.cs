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
    
    public partial class role
    {
        public role()
        {
            this.users = new HashSet<user>();
        }
    
        public sbyte role_id { get; set; }
        public string role_name { get; set; }
        public string role_description { get; set; }
        public System.DateTime role_created { get; set; }
        public Nullable<System.DateTime> role_modified { get; set; }
        public Nullable<System.DateTime> role_deleted { get; set; }
    
        public virtual ICollection<user> users { get; set; }
    }
}
