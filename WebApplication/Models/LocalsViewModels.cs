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
        public DishesViewModels menu { get; set; }
    }

    public class DishesRegisterModels
    {
        public Restaurant restaurant { get; set; }
        public Dish dish { get; set; }
    }


    public class DishesViewModels
    {
        public Restaurant restaurant { get; set; }
        public IEnumerable<Dish> dishes { get; set; }
    }
}
