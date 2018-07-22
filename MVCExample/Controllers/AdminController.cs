using MVCExample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net;

namespace MVCExample.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(int ? id )
        {
            if (id != null)
            {

                DiabetEntities db = new DiabetEntities();

                IEnumerable<Login> isadmin = from k in db.Login where k.UserId == id select k;
                if (isadmin.First().Username == "admin")
                {

                    IEnumerable<Login> logins = from s in db.Login where s.TypeApproval == 0 select s;
                    return View(logins);
                }
                else
                    return RedirectToAction("Index", "Login");
            }

            else return RedirectToAction("Index", "Login");
        }
       [HttpPost]
        public ActionResult Accept(int? id )
        { if (id != null)
            {
                using (DiabetEntities db = new DiabetEntities())
                {
                    IEnumerable<Login> logins = from s in db.Login where s.UserId == id select s;
                    logins.Single().TypeApproval = logins.Single().UserDetail.Type;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Admin");
                }
            }
        else return RedirectToAction("Index", "Admin");
        }
        private string GetCookie(string name)
        {
            //Böyle bir cookie mevcut mu kontrol ediyoruz
            if (Request.Cookies.AllKeys.Contains(name))
            {
                //böyle bir cookie varsa bize geri değeri döndürsün
                return Request.Cookies[name].Value;
            }
            return null;
        }
    }
}