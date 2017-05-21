using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tabemashou_User.Models;
using Type = Tabemashou_User.Models.Type;

namespace Tabemashou_User.Controllers
{
    public class RestaurantsController : Controller
    {
        private TabemashouEntities db = new TabemashouEntities();

        
        public ActionResult Index()
        {
            MapView model = new MapView();
            model.Locals = db.Local.Take(100).ToList();
            model.Types = new List<TypeFilter>();
            foreach (var typeTmp in db.PR_RestaurantTypes().ToList())
            {
                TypeFilter tmp = new TypeFilter();
                tmp.TypeMap = db.Type.Find(typeTmp.IdType);
                tmp.TypeId = typeTmp.IdType;
                tmp.CheckFilter = true;
                model.Types.Add(tmp);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(MapView model)
        {

            List<Models.Type> types = new List<Models.Type>();
            List<District> districtFilter = new List<District>();

            MapView newModel = new MapView();
            newModel.Types = new List<TypeFilter>();
            newModel.Locals = db.Local.ToList();

            if (model.IdDistrict != null)
            {
                foreach (var id in model.IdDistrict)
                {
                    District tmp = db.District.Find(id);
                    districtFilter.Add(tmp);
                }
                newModel.Locals = newModel.Locals.Where(m => districtFilter.Contains(m.District)).ToList();

            }

            if (model.Types != null) { 
                foreach (var tmp in model.Types)
                {
                    Type tmpType = db.Type.Find(tmp.TypeId);
                    Debug.WriteLine(tmp.CheckFilter);
                    Debug.WriteLine(tmpType.Name);
                    if (tmp.CheckFilter)
                    {
                        types.Add(tmpType);
                    }
                    TypeFilter tmp2 = new TypeFilter();
                    tmp2.TypeMap = tmpType;
                    tmp2.TypeId = tmpType.IdType;
                    tmp2.CheckFilter = tmp.CheckFilter;
                    newModel.Types.Add(tmp2);
                }
                newModel.Locals = newModel.Locals.Where(m => m.Restaurant.Type.Any(types.Contains)).ToList();
            }
           
            
            return View(newModel);
        }
    }
}
