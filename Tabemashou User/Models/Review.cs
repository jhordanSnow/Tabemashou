//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Tabemashou_User.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public int IdReview { get; set; }
        public System.DateTime Date { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Quality { get; set; }
        public decimal IdCustomer { get; set; }
        public int IdCheck { get; set; }
    
        public virtual Check Check { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
