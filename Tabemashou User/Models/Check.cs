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
    
    public partial class Check
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Check()
        {
            this.DishesByCheck = new HashSet<DishesByCheck>();
            this.PaymentByCustomer = new HashSet<PaymentByCustomer>();
            this.Review = new HashSet<Review>();
        }
    
        public int IdCheck { get; set; }
        public int IdLocal { get; set; }
        public System.DateTime Date { get; set; }
        public string State { get; set; }
        public decimal Balance { get; set; }
        public int intState { get; set; }

        public virtual Local Local { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DishesByCheck> DishesByCheck { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentByCustomer> PaymentByCustomer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Review { get; set; }

        public decimal getSubtotal()
        {
            decimal subTotal = 0;
            foreach (DishesByCheck dish in DishesByCheck)
            {
                subTotal += dish.UnitaryPrice * dish.Quantity;
            }
            return subTotal;
        }

        public decimal getSellTax()
        {
            decimal sellTax = 0;
            foreach (DishesByCheck dish in DishesByCheck)
            {
                sellTax += dish.SellTax;
            }
            return sellTax / DishesByCheck.Count;
        }

        public decimal getServiceTax()
        {
            decimal serviceTax = 0;
            foreach (DishesByCheck dish in DishesByCheck)
            {
                serviceTax += dish.ServiceTax;
            }
            return serviceTax / DishesByCheck.Count;
        }

        public decimal getSellTaxTotal()
        {
            decimal subTotal = 0;
            foreach (DishesByCheck dish in DishesByCheck)
            {
                subTotal += dish.UnitaryPrice * dish.Quantity * dish.SellTax / 100;
            }
            return subTotal;
        }

        public decimal getServiceTaxTotal()
        {
            decimal subTotal = 0;
            foreach (DishesByCheck dish in DishesByCheck)
            {
                subTotal += dish.UnitaryPrice * dish.Quantity * dish.ServiceTax / 100;
            }
            return subTotal;
        }
        public decimal getTotal()
        {
            decimal total = 0;
            foreach (DishesByCheck dish in DishesByCheck)
            {
                total += dish.getTotal() ;
            }
            return total;
        }



    }
}
