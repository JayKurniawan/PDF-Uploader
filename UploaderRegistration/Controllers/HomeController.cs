using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploaderRegistration.Models;

namespace UploaderRegistration.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        // create object from User class
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                UploaderDatabaseEntities db = new UploaderDatabaseEntities();
                db.Users.Add(user);
                db.SaveChanges();
            }

            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                using(UploaderDatabaseEntities db = new UploaderDatabaseEntities())
                {
                    var obj = db.Users.Where(a => a.Username.Equals(user.Username) && a.Password.Equals(user.Password)).FirstOrDefault();                    
                    if(obj != null)
                    {
                        Session["UserId"] = obj.UserId.ToString();
                        Session["Username"] = obj.Username.ToString();

                        // go to dashboard
                        return RedirectToAction("UserDashboard");
                    }
                }
            }
            return View(user);
        }

        
        public ActionResult UserDashboard()
        {
            if(Session["UserId"] != null){
                

                
                
                // go to dashboard
                return View();
            }else{
                // go to login page
                return RedirectToAction("Login");
            }
        }
        
        [HttpPost]
        public ActionResult UserDashboard(HttpPostedFileBase file)
        {
                    if (file != null)
                    {
                        string path = Server.MapPath("~/Files/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        file.SaveAs(path + Path.GetFileName(file.FileName));
                        ViewBag.Message = "Uploaded.";
                    }

            return View();
        }


    }
}