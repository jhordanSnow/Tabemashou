using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tabemashou_User.Models
{
    public class CommonClasses
    {
    }

    public class userCheck
    {
        public int Code { get; set; }
        public bool Correct { get; set; }
        public Table CheckTable { get; set; }
        public Restaurant CheckRestaurant { get; set; }
        public Local CheckLocal { get; set; }
        public string DistrictCompeteName { get; set; }
        public string UserDishes { get; set; }
        public List<Type> types { get; set; }
    }

    public class UserAdd
    {
        public string UserName { get; set; }
        public int CheckId { get; set; }
        public decimal UserId { get; set; }
        public decimal TotalPay { get; set; }
    }
}