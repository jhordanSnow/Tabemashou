//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
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
