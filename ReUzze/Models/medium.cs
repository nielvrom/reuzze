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
    
    public partial class medium
    {
        public int medium_id { get; set; }
        public long entity_id { get; set; }
        public string medium_url { get; set; }
        public string medium_type { get; set; }
        public string medium_mimetype { get; set; }
        public Nullable<bool> medium_isexternal { get; set; }
    
        public virtual entity entity { get; set; }
    }
}