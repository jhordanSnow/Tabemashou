//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Restaurant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Restaurant()
        {
            this.Dish = new HashSet<Dish>();
            this.Local = new HashSet<Local>();
            this.Type = new HashSet<Type>();
        }
    
        public int IdRestaurant { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "The name must have less than 50 characters")]
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public decimal IdAdmin { get; set; }
    
        public virtual Administrator Administrator { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dish> Dish { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Local> Local { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Type> Type { get; set; }
    }
}
