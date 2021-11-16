using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_gallery.Models
{
    public class UserController : Controller
    {
        // GET: User


        UsersModel db = new UsersModel();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(user us)
        {
            db.users.Add(us);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
     
            return View();

        }
        [HttpPost]
        public ActionResult Login(user us)
        {
            var obj = db.users.Where(x => x.username.Equals(us.username) && x.password.Equals(us.password)).FirstOrDefault();
            {
                if(obj != null)
                {
                    return RedirectToAction("Employ");
                }
                else if (us.username == "Admin"  && us.password == "Admin")
                {
                    return RedirectToAction("Admin");
                }
                return View();
            }

        }

        public ActionResult Employ()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }


    }
}