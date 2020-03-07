using ChainOfResponsibility.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChainOfResponsibility.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public JsonResult customer(int id)
        {
            var customers = new Customer[]{
                                new Customer { id = 1, firstname = "Hamed", lastname = "Karimi" },
                                new Customer { id = 2, firstname = "Ali", lastname = "Saberi" },
                                new Customer { id = 3, firstname = "Reza", lastname = "Mahmoodi" },
                                new Customer { id = 4, firstname = "Pedram", lastname = "Shayan" },
                                new Customer { id = 5, firstname = "Naser", lastname = "Kiani" },
                            };
            var a = customers.Where(x => x.id == id).SingleOrDefault();
            if (a != null)
                return Json(a, JsonRequestBehavior.AllowGet);
            else
                return Json(new Customer(), JsonRequestBehavior.AllowGet);
        }
    }
}
