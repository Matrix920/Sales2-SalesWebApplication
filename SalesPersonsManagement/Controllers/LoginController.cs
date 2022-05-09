using Sales_Persons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Sales_Persons.Controllers
{
    public class LoginController : Controller
    {
        SalesDBEntities2 db = new SalesDBEntities2();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // post: Login
        [HttpPost]
        public ActionResult Login(String username,String password)
        {
            var admin = db.Admins.Where(x => x.Username.Equals(username) && x.Password.Equals(password)).SingleOrDefault();
            if (admin != null)
            {
                Session["admin"] = true;
                Session["person"] = admin;

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                var person = db.SalesPersons.Where(x => x.username.Equals(username) && x.Password.Equals(password)).SingleOrDefault();

                if (person != null)
                {
                    Session["admin"] = false;
                    Session["person"] = person;
                    return RedirectToAction("Index", "SalesPerson");
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult LoginAndroid(String Username, String Password)
        {
            var admin = db.Admins.Where(x => x.Username.Equals(Username) && x.Password.Equals(Password)).SingleOrDefault();
            if (admin != null)
            {
                var r = new
                {
                    Login = true,
                    Admin = true
                };
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var person = db.SalesPersons.Where(x => x.username.Equals(Username) && x.Password.Equals(Password)).SingleOrDefault();

                if (person != null)
                {
                    var r1 = new
                    {
                        Login = true,
                        Admin = false,
                        PersonID=person.PersonID,
                        RegionID=person.RegionID,
                        FullName=person.FullName,
                        RegisterationDate=person.RegisterationDate,
                        RegionName=person.Region.RegionName,
                        Picture=person.Picture
                    };

                    return Json(r1, JsonRequestBehavior.AllowGet);
                }
            }
            var r2= new
            {
               Login=false
            };
            return Json(r2, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login");
        }
    }
}