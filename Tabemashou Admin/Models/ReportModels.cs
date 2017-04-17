using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Tabemashou_Admin.Models
{
    public class ReportLocal
    {
        public SelectList Restaurants { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int RestaurantId { get; set; }
    }
}
