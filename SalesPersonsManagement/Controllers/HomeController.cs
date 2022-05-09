using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Persons.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Random()
        {/*
            SalesPerson person;
            List<User> users;
            using(var db=new SalesDBEntities())
            {
                person = (from p in db.SalesPersons
                          where p.PersonID == 1
                          select p).Single();

                users = db.Users.ToList();
            }
            */
            // return View(person);
            //return Content("hello world");
            //return HttpNotFound();
            //return new EmptyResult();
            //ViewData["person"] = person;
            return View();
            //return RedirectToAction("index", "HomeController", new { id = 1, page = "fuck" });
        }

        public ActionResult Edit(int movieID)
        {
            return Content("id=" + movieID);
        }

        public ActionResult Index(int? pageIndex,String sortBy)
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;
            if (String.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";
            return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        }

        [Route("Home/released/{year}/{month:regex(\\d{4}):range(1,12)}")]
        public ActionResult ByReleasedDate(int year,int month)
        {
            return Content(year+"/"+month);
        }
    }
}