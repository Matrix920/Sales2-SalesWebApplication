using Sales_Persons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Persons.Controllers
{
    public class SalesPersonController : Controller
    {
        SalesDBEntities2 _db = new SalesDBEntities2();

        // GET: SalesPerson
        public ActionResult Index()
        {
            if (Session["person"] != null)
            {
                if (!(Boolean)Session["admin"])
                {
                    var p = (SalesPerson)Session["person"];
                    return View(p);
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            return RedirectToAction("Login", "Login");
            
        }
        //String txtSouth, String txtNorth, String txtCostal, String txtEast, String txtLeban

        public ActionResult CalculateComission()
        {
            return View();
        }
 
        [HttpPost]
        public ActionResult CalculateComission(String txtSouth, String txtNorth, String txtCostal, String txtEast, String txtLeban,String txtMonth,String txtYear)
        {
            try {
                double south = Convert.ToDouble(txtSouth);
                double north =Convert.ToDouble(txtNorth);
                double east = Convert.ToDouble(txtEast);
                double costal = Convert.ToDouble(txtCostal);
                double leban =  Convert.ToDouble(txtLeban);

                double[] sales = new double[5] { south, north, east, costal, leban };

                int month = Convert.ToInt16(txtMonth);
                int year = Convert.ToInt16(txtYear);

                var person = (SalesPerson)Session["person"];
                double[] commission = new double[6];

                switch (person.RegionID)
                {
                    case 1:
                        commission[1] = south > 1000000 ? 1000000 * 0.05 + (south - 1000000) * 0.07 : south * 0.05;
                        commission[2] = north > 1000000 ? 1000000 * 0.03 + (north - 1000000) * 0.04 : north * 0.03;
                        commission[3] = costal > 1000000 ? 1000000 * 0.03 + (costal - 1000000) * 0.04 : costal * 0.03;
                        commission[4] = east > 1000000 ? 1000000 * 0.03 + (east - 1000000) * 0.04 : east * 0.03;
                        commission[5] = leban > 1000000 ? 1000000 * 0.03 + (leban - 1000000) * 0.04 : leban * 0.03;
                        break;
                    case 2:
                        commission[1] = south > 1000000 ? 1000000 * 0.03 + (south - 1000000) * 0.04 : south * 0.03;
                        commission[2] = north > 1000000 ? 1000000 * 0.05 + (north - 1000000) * 0.07 : north * 0.05;
                        commission[3] = costal > 1000000 ? 1000000 * 0.03 + (costal - 1000000) * 0.04 : costal * 0.03;
                        commission[4] = east > 1000000 ? 1000000 * 0.03 + (east - 1000000) * 0.04 : east * 0.03;
                        commission[5] = leban > 1000000 ? 1000000 * 0.03 + (leban - 1000000) * 0.04 : leban * 0.03;
                        break;
                    case 3:
                        commission[1] = south > 1000000 ? 1000000 * 0.03 + (south - 1000000) * 0.04 : south * 0.03;
                        commission[2] = north > 1000000 ? 1000000 * 0.03 + (north - 1000000) * 0.04 : north * 0.03;
                        commission[3] = costal > 1000000 ? 1000000 * 0.05 + (costal - 1000000) * 0.07 : costal * 0.05;
                        commission[4] = east > 1000000 ? 1000000 * 0.03 + (east - 1000000) * 0.04 : east * 0.03;
                        commission[5] = leban > 1000000 ? 1000000 * 0.03 + (leban - 1000000) * 0.04 : leban * 0.03;
                        break;
                    case 4:
                        commission[1] = south > 1000000 ? 1000000 * 0.03 + (south - 1000000) * 0.04 : south * 0.03;
                        commission[2] = north > 1000000 ? 1000000 * 0.03 + (north - 1000000) * 0.04 : north * 0.03;
                        commission[3] = costal > 1000000 ? 1000000 * 0.03 + (costal - 1000000) * 0.04 : costal * 0.03;
                        commission[4] = east > 1000000 ? 1000000 * 0.05 + (east - 1000000) * 0.07 : east * 0.05;
                        commission[5] = leban > 1000000 ? 1000000 * 0.03 + (leban - 1000000) * 0.04 : leban * 0.03;
                        break;
                    case 5:
                        commission[1] = south > 1000000 ? 1000000 * 0.03 + (south - 1000000) * 0.04 : south * 0.03;
                        commission[2] = north > 1000000 ? 1000000 * 0.03 + (north - 1000000) * 0.04 : north * 0.03;
                        commission[3] = costal > 1000000 ? 1000000 * 0.03 + (costal - 1000000) * 0.04 : costal * 0.03;
                        commission[4] = east > 1000000 ? 1000000 * 0.03 + (east - 1000000) * 0.04 : east * 0.03;
                        commission[5] = leban > 1000000 ? 1000000 * 0.05 + (leban - 1000000) * 0.07 : leban * 0.05;
                        break;

                }

                double fullCom = commission.Sum();

                DateTime date = new DateTime(year, month, 1);
                //save commission
                _db.Comissions.Add(new Comission
                {
                    Comm = Convert.ToDecimal(fullCom),
                    Date = date,
                    PersonID = person.PersonID

                });

                //save sale to person
                var sale = new Sale
                {
                    PersonID = person.PersonID,
                    Date = date,
                };

                _db.Sales.Add(sale);
                _db.SaveChanges();

                int saleID = sale.SaleID;
                //save sale details
                var salesDetails = new List<SalesDetail>();
                for (int i = 1; i <= 5; i++)
                {
                    salesDetails.Add(new SalesDetail
                    {
                        RegionID = i,
                        Sell = Convert.ToDecimal(commission[i]),
                        SaleID = saleID
                    });
                }


                _db.SalesDetails.AddRange(salesDetails);
                _db.SaveChanges();

                ViewData["error"] = commission;
                return RedirectToAction("Details",new { id = saleID });
            }
            catch(Exception e)
            {
                ViewData["error"] = e.Message;
                return View();
            }
        }


        [HttpPost]
        public ActionResult CalculateComissionAndroid(String South, String North, String Costal, String East, String Lebanon, String Month, String Year,String PersonID,String RegionID)
        {
            try
            {
                double south = Convert.ToDouble(South);
                double north = Convert.ToDouble(North);
                double east = Convert.ToDouble(East);
                double costal = Convert.ToDouble(Costal);
                double leban = Convert.ToDouble(Lebanon);

                int personID = Int32.Parse(PersonID);
                int regionID = Int32.Parse(RegionID);

                double[] sales = new double[] { south, north, east, costal, leban };

                int month = Convert.ToInt16(Month);
                int year = Convert.ToInt16(Year);

                double[] commission = new double[6];

                switch (regionID)
                {
                    case 1:
                        commission[1] = south > 1000000 ? 1000000 * 0.05 + (south - 1000000) * 0.07 : south * 0.05;
                        commission[2] = north > 1000000 ? 1000000 * 0.03 + (north - 1000000) * 0.04 : north * 0.03;
                        commission[3] = costal > 1000000 ? 1000000 * 0.03 + (costal - 1000000) * 0.04 : costal * 0.03;
                        commission[4] = east > 1000000 ? 1000000 * 0.03 + (east - 1000000) * 0.04 : east * 0.03;
                        commission[5] = leban > 1000000 ? 1000000 * 0.03 + (leban - 1000000) * 0.04 : leban * 0.03;
                        break;
                    case 2:
                        commission[1]= south > 1000000 ? 1000000 * 0.03 + (south - 1000000) * 0.04 : south * 0.03;
                        commission[2] = north > 1000000 ? 1000000 * 0.05 + (north - 1000000) * 0.07 : north * 0.05;
                       commission[3]  = costal > 1000000 ? 1000000 * 0.03 + (costal - 1000000) * 0.04 : costal * 0.03;
                        commission[4] = east > 1000000 ? 1000000 * 0.03 + (east - 1000000) * 0.04 : east * 0.03;
                        commission[5] = leban > 1000000 ? 1000000 * 0.03 + (leban - 1000000) * 0.04 : leban * 0.03;
                        break;
                    case 3:
                        commission[1] = south > 1000000 ? 1000000 * 0.03 + (south - 1000000) * 0.04 : south * 0.03;
                        commission[2] = north > 1000000 ? 1000000 * 0.03 + (north - 1000000) * 0.04 : north * 0.03;
                        commission[3] = costal > 1000000 ? 1000000 * 0.05 + (costal - 1000000) * 0.07 : costal * 0.05;
                        commission[4] = east > 1000000 ? 1000000 * 0.03 + (east - 1000000) * 0.04 : east * 0.03;
                        commission[5] = leban > 1000000 ? 1000000 * 0.03 + (leban - 1000000) * 0.04 : leban * 0.03;
                        break;
                    case 4:
                        commission[1] = south > 1000000 ? 1000000 * 0.03 + (south - 1000000) * 0.04 : south * 0.03;
                        commission[2] = north > 1000000 ? 1000000 * 0.03 + (north - 1000000) * 0.04 : north * 0.03;
                        commission[3] = costal > 1000000 ? 1000000 * 0.03 + (costal - 1000000) * 0.04 : costal * 0.03;
                        commission[4] = east > 1000000 ? 1000000 * 0.05 + (east - 1000000) * 0.07 : east * 0.05;
                        commission[5] = leban > 1000000 ? 1000000 * 0.03 + (leban - 1000000) * 0.04 : leban * 0.03;
                        break;
                    case 5:
                        commission[1] = south > 1000000 ? 1000000 * 0.03 + (south - 1000000) * 0.04 : south * 0.03;
                        commission[2] = north > 1000000 ? 1000000 * 0.03 + (north - 1000000) * 0.04 : north * 0.03;
                        commission[3] = costal > 1000000 ? 1000000 * 0.03 + (costal - 1000000) * 0.04 : costal * 0.03;
                        commission[4] = east > 1000000 ? 1000000 * 0.03 + (east - 1000000) * 0.04 : east * 0.03;
                        commission[5] = leban > 1000000 ? 1000000 * 0.05 + (leban - 1000000) * 0.07 : leban * 0.05;
                        break;

                }
                
                double fullCom = commission.Sum();

                DateTime date = new DateTime(year, month, 1);
                //save commission
                var comission=new Comission
                {
                    Comm = Convert.ToDecimal(fullCom),
                    Date = date,
                    PersonID = personID

                };

                _db.Comissions.Add(comission);

                //save sale to person
                var sale = new Sale
                {
                    PersonID = personID,
                    Date = date,
                };

                _db.Sales.Add(sale);
                _db.SaveChanges();

                int saleID = sale.SaleID;
                //save sale details
                var salesDetails = new List<SalesDetail>();
                for (int i = 1; i <= 5; i++)
                {
                    salesDetails.Add(new SalesDetail
                    {
                        RegionID = i ,
                        Sell = Convert.ToDecimal(commission[i]),
                        SaleID = saleID
                    });
                }


                _db.SalesDetails.AddRange(salesDetails);
                _db.SaveChanges();

                return Json(new { Error = false,SaleID=saleID,CommisionID=comission.ComissionID }, JsonRequestBehavior.AllowGet);


            }
            catch(Exception e)
            {
                return Json(new { Error = true ,Messge=e.Message,stack=e.StackTrace}, JsonRequestBehavior.AllowGet);
            }

           
        }

        // GET: SalesPerson/Details/5
        public ActionResult Details(int id)
        {
            var saleDetails = (from s in _db.Sales
                               where s.SaleID == id
                               select new
                               {
                                   number = s.SalesPerson.PersonID,
                                   name = s.SalesPerson.FullName,
                                   month = s.Date.Month,
                                   year = s.Date.Year,
                                   regDate = s.SalesPerson.RegisterationDate,
                                   com = s.Comission.Comm,
                                   sDetails = s.SalesDetails.ToList()
                               }).FirstOrDefault();
            if (saleDetails != null) {
                ViewData["number"] = saleDetails.number;
                ViewData["name"] = saleDetails.name;
                ViewData["month"] = saleDetails.month;
                ViewData["year"] = saleDetails.year;
                ViewData["regDate"] = Convert.ToString(saleDetails.regDate);
                ViewData["comm"] = saleDetails.com;
                ViewData["south"] = saleDetails.sDetails.Where(x => x.RegionID == 1).Select(x=>x.Sell).SingleOrDefault();
                ViewData["north"] = saleDetails.sDetails.Where(x => x.RegionID == 2).Select(x => x.Sell).SingleOrDefault();
                ViewData["costal"] = saleDetails.sDetails.Where(x => x.RegionID == 3).Select(x => x.Sell).SingleOrDefault();
                ViewData["east"] = saleDetails.sDetails.Where(x => x.RegionID == 4).Select(x => x.Sell).SingleOrDefault();
                ViewData["leban"] = saleDetails.sDetails.Where(x => x.RegionID == 5).Select(x => x.Sell).SingleOrDefault();
            }
            else
            {
                ViewData["error"] = "error";
            }
            return View();
        }

        public ActionResult DetailsAndroid(int id)
        {
            var saleDetails = (from s in _db.Sales
                               where s.SaleID == id
                               select new
                               {
                                   PersonID = s.SalesPerson.PersonID,
                                   FullName = s.SalesPerson.FullName,
                                   Month = s.Date.Month,
                                   Year = s.Date.Year,
                                   regDate =s.SalesPerson.RegisterationDate,
                                   Comm = s.Comission.Comm,
                                   South = s.SalesDetails.Where(x => x.RegionID == 1).Select(x => x.Sell).FirstOrDefault(),
                                   North = s.SalesDetails.Where(x => x.RegionID == 2).Select(x => x.Sell).FirstOrDefault(),
                                   Costal = s.SalesDetails.Where(x => x.RegionID == 3).Select(x => x.Sell).FirstOrDefault(),
                                   East = s.SalesDetails.Where(x => x.RegionID == 4).Select(x => x.Sell).FirstOrDefault(),
                                   Lebanon = s.SalesDetails.Where(x => x.RegionID == 5).Select(x => x.Sell).FirstOrDefault()

                               }).FirstOrDefault();
            return Json(saleDetails,JsonRequestBehavior.AllowGet);
        }
        /*
        // GET: SalesPerson/Create
        public ActionResult Create()
        {
            return View();
        }
        */
        // POST: SalesPerson/Create
        [HttpPost]
        public ActionResult Create(String txtSouth, String txtNorth, String txtCostal, String txtEast, String txtLeban)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SalesPerson/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SalesPerson/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SalesPerson/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SalesPerson/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
