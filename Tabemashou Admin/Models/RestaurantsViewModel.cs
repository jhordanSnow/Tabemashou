using System.Collections.Generic;
using System.Web.Mvc;

namespace Tabemashou_Admin.Models
{
    public class RestaurantsViewModel
    {
        public IEnumerable<PR_RestaurantInfo_Result> restaurant { get; set; }
    }

    public class RegisterRestaurantModel
    {
        public Restaurant restaurant { get; set; }
        public System.Type tipos { get; set; }
        public List<System.Type> restTypes { get; set; }
        public int[] restTypesId { get; set; }
        public MultiSelectList selectedItems { get; set; }
    }
}