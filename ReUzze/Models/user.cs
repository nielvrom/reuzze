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
    
    public partial class user
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public string user_email { get; set; }
        public int user_rating { get; set; }
        public System.DateTime user_created { get; set; }
        public Nullable<System.DateTime> user_modified { get; set; }
        public Nullable<System.DateTime> user_deleted { get; set; }
        public Nullable<System.DateTime> user_lastlogin { get; set; }
        public Nullable<System.DateTime> user_locked { get; set; }
        public int person_id { get; set; }
        public sbyte role_id { get; set; }
    
        public virtual person person { get; set; }
        public virtual role role { get; set; }
    }
}