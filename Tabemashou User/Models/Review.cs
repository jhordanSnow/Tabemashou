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
    
    public partial class Review
    {
        public int IdReview { get; set; }
        public System.DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Quality { get; set; }
        public decimal IdCustomer { get; set; }
        public int IdLocal { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Local Local { get; set; }
    }
}