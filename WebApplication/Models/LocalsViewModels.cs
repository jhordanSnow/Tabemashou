using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class LocalsViewModels
    {
        public Restaurant restaurant { get; set; }
        public IEnumerable<Local> locals { get; set; }
        public IEnumerable<Dish> menu { get; set; }
    }
}