using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class RestaurantsViewModel
    {
        public IEnumerable<RestaurantInfo_Result> restaurant { get; set; }
    }

    public class RegisterRestaurantModel
    {
        public Restaurant restaurant { get; set; }
        public Type tipos { get; set; }
        public List<Type> restTypes { get; set; }
        public int[] restTypesId { get; set; }
    }
}