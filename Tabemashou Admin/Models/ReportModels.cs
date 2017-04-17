using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Web.Mvc;

namespace Tabemashou_Admin.Models
{
    public class ReportLocal
    {
        public SelectList Restaurants { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public int RestaurantId { get; set; }
        public int Top { get; set; }
        public List<ReportLocalResult> Result { get; set; }
    }

    public class ReportLocalResult
    {
        public Local LocalResult { get; set; }
        public int SalesResult { get; set; }
        public DateTime DayResult { get; set; }
        public decimal TotalResult { get; set; }
    }
}
