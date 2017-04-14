using System.Collections.Generic;

namespace Tabemashou_Admin.Models
{
    public class LocalsViewModels
    {
        public Restaurant restaurant { get; set; }
        public IEnumerable<Local> locals { get; set; }
        public DishesViewModels menu { get; set; }
    }


    public class localRegister
    {
        public Restaurant restaurant { get; set; }
        public int idRestaurant { get; set; }
        public Local local { get; set; }
        public List<DishLocal> menu { get; set; }
        public IEnumerable<Photo> photos { get; set; }
        public string uploadFilesNames { get; set; }
        public string deletedFilesIds { get; set; }
        public int cantMesas { get; set; }
    }

    public class DishLocal
    {
        public Dish dish { get; set; }
        public bool state { get; set; }
        public int idDish { get; set; }
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
