//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tabemashou_User.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Table
    {
        public int IdTable { get; set; }
        public string DistinctiveName { get; set; }
        public int IdLocal { get; set; }
    
        public virtual Local Local { get; set; }
    }
}