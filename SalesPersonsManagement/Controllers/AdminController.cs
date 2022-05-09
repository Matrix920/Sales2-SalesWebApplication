using Sales_Persons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Persons.Controllers
{
    public class AdminController : Controller
    {
        SalesDBEntities2 db = new SalesDBEntities2();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["person"] != null)
            {
                if ((Boolean)Session["admin"]) {
                    var salesPersons = db.SalesPersons.ToList();
                    return View(salesPersons);
                }
                else
                {
                    return RedirectToAction("Index", "SalesPerson");
                }
            }
            return RedirectToAction("Login", "Login");
            
        }


        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            var salesPerson = db.SalesPersons.Find(id);
            return View(salesPerson);
        }
        public ActionResult Edit(int id)
        {
            var salesPerson = db.SalesPersons.Find(id);
            return View(salesPerson);
        }

        [HttpPost]
        public ActionResult AddSalesPersonAndroid(String RegionID,String FullName,String Month,String Year,String Username,String Password)
        {
            db.SalesPersons.Add(new SalesPerson
            {
                FullName = FullName,
                Password = Password,
                username=Username,
                RegionID = Int32.Parse(RegionID),
                RegisterationDate = new DateTime(Int32.Parse(Year), Int32.Parse(Month),1),
            });

            db.SaveChanges();

            return Json(new { Error = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeletePersonAndroid(String PersonID)
        {
            try {
                int id = Int32.Parse(PersonID);

                var p = db.SalesPersons.Find(id);
                db.SalesPersons.Remove(p);
                db.SaveChanges();

                return Json(new { Error = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Error = true,Message="This person has sales" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EditAndroid(String RegionID, String FullName, String Month, String Year, String Username, String Password, String PersonID)
        {
            int id = Int32.Parse(PersonID);
            int month = Int32.Parse(Month);
            int year = Int32.Parse(Year);
            int regionID = Int32.Parse(RegionID);
            var person = db.SalesPersons.Find(id);

            person.FullName = FullName;
            person.RegionID = regionID;
            person.RegisterationDate = new DateTime(year, month, 1);
            db.SaveChanges();

            return Json(new { Error = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(SalesPerson salesPerson,HttpPostedFile imgFile)
        {
            var person = db.SalesPersons.Find(salesPerson.PersonID);

            if (person != null)
            {
                if (imgFile != null && imgFile.ContentLength > 0)
                {
                    string ImageName = System.IO.Path.GetFileName(imgFile.FileName); 
                    string physicalPath = Server.MapPath("~/Pictures/" + ImageName);

                    // save image in folder  
                    imgFile.SaveAs(physicalPath);
                    person.Picture = "Pictures/" + ImageName;


                }
                person.username = salesPerson.username;
                person.Password = salesPerson.Password;
                person.FullName = salesPerson.FullName;
                person.RegionID = salesPerson.RegionID;
                person.RegisterationDate = salesPerson.RegisterationDate;
                db.SaveChanges();
                
            }
            return RedirectToAction("Details", new { id = person.PersonID });
        }
        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(String txtID,String txtMonth,String txtYear)
        {
            int id = Int32.Parse(txtID);
            int month = Int32.Parse(txtMonth);
            int year = Int32.Parse(txtYear);

            var commissions = db.Comissions.Where(x => x.PersonID == id && x.Date.Month==month && x.Date.Year==year).ToList();
            return View(commissions);
        }

        [HttpPost]
        public ActionResult GetComissionsByDateAndroid(String PersonID, String Month, String Year)
        {
            int id = Int32.Parse(PersonID);
            int month = Int32.Parse(Month);
            int year = Int32.Parse(Year);

            var commissions = db.Comissions.Where(x => x.PersonID == id && x.Date.Month == month && x.Date.Year == year).Select
                (x => new
                {
                    ComissionID = x.ComissionID,
                    Month = x.Date.Month,
                    Year = x.Date.Year,
                    Comm = x.Comm
                }).ToList();

            return Json(commissions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CommDetails(int id)
        {
            int salesID = db.Sales.Where(x => x.ComissionID == id).FirstOrDefault().SaleID;
            return RedirectToAction("Details", "SalesPerson", new { id = salesID });
        }

        public ActionResult CommDetailsAndroid(int id)
        {
            int salesID = db.Sales.Where(x => x.ComissionID == id).FirstOrDefault().SaleID;
            return RedirectToAction("DetailsAndroid", "SalesPerson", new { id = salesID });
        }

        public ActionResult GetSalesPersonsAndroid()
        {
            var salesPersons = from s in db.SalesPersons
                               select new
                               {
                                   PersonID = s.PersonID,
                                   FullName = s.FullName,
                                   Month = s.RegisterationDate.Value.Month,
                                   Year = s.RegisterationDate.Value.Year,
                                   RegionID = s.RegionID,
                                   RegionName = s.Region.RegionName,
                                   Username=s.username,
                                   Password=s.Password,
                                   Picture=s.Picture
                               };

            return Json(salesPersons, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(int id)
        {
            var commissions = db.Comissions.Where(x => x.PersonID == id).ToList();
            return View(commissions);
        }

        public ActionResult GetPersonsComissionsAndroid(int id)
        {
            var commissions = db.Comissions.Where(x => x.PersonID == id).Select
                (x => new
                {
                    ComissionID = x.ComissionID,
                    Month = x.Date.Month,
                    Year = x.Date.Year,
                    Comm = x.Comm
                }).ToList();

            return Json(commissions, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Create

            [HttpPost]
        public ActionResult Create(SalesPerson salesPerson, HttpPostedFileBase imgFile)
        {
            if (!ModelState.IsValid)
                return View();
            if (imgFile != null && imgFile.ContentLength > 0)
            {
                string ImageName = System.IO.Path.GetFileName(imgFile.FileName); 
                string physicalPath = Server.MapPath("~/Pictures/" + ImageName);

                 // save image in folder  
                imgFile.SaveAs(physicalPath);
                salesPerson.Picture = "Pictures/" + ImageName;

                
            }
            db.SalesPersons.Add(salesPerson);
            db.SaveChanges();

            return RedirectToAction("Index");
        } 
            
       
        
        // GET: Admin/Edit/5
     

        

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            var salesPerson = db.SalesPersons.Find(id);
            
            return View(salesPerson);
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, SalesPerson salesPerson)
        {
            try
            {
                var person = db.SalesPersons.Find(id);
                db.SalesPersons.Remove(person);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
