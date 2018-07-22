using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCExample.Models;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MVCExample.Controllers
{
    public class HomeController : Controller
    {
        private DiabetEntities db = new DiabetEntities();
        // GET: 
        private static int? userid = 0;
        private static int dietlistid;
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                if (id.ToString() == GetCookie("userid"))
                {
                    userid = id;
                    // join yapıp bizim gönderdiklerimizle bize gönderilenler birleştirilecek 
                    IEnumerable<Login> login = from t in db.Login where t.UserId == id select t;
                    IEnumerable<RequestContactList> requestContactLists = from req in db.RequestContactList where req.ContactId == id && req.status == 0 select req;
                    IEnumerable<ContactList> contacts = from cont in db.ContactList where cont.OwnerId == id select cont;
                    IEnumerable<DietList> dlist = from dl in db.DietList where dl.GiveToId == (int)userid orderby dl.DietListDate descending select dl;
                    IEnumerable<Message> msg = from m in db.Message where m.SenderId == id && m.IsRead == 0 select m;
                    int notificiation = msg.Count();
                    ViewBag.notify = notificiation.ToString();
                    var tuple = new Tuple<Login, IEnumerable<RequestContactList>, IEnumerable<ContactList>, IEnumerable<DietList>>(login.First(), requestContactLists, contacts, dlist);


                    return View(tuple);
                }

                else
                    return RedirectToAction("Index", "Home", new { id = GetCookie("userid") });
            }
            else if (GetCookie("userid") != null)
            {
                return RedirectToAction("Index", "Home", new { id = GetCookie("userid") });
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult AcceptRequest(int? acceptid)
        {

            if (acceptid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestContactList contact = db.RequestContactList.Find(acceptid);

            if (contact == null)
            {
                return HttpNotFound();
            }
            contact.status = 1;

            db.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Files"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }
        public ActionResult Matched()
        {
            IEnumerable<Login> login = from t in db.Login where t.UserId == userid select t;

            IEnumerable<UserDetail> searchresult = from s in db.UserDetail where (s.BMI >= login.FirstOrDefault().UserDetail.BMI -10 ) && (s.BMI <= login.FirstOrDefault().UserDetail.BMI + 10) && s.Type == 1 && s.InsulinUsage ==login.FirstOrDefault().UserDetail.InsulinUsage && s.Gender == login.FirstOrDefault().UserDetail.Gender && s.UserId != login.FirstOrDefault().UserId select s;
            var tuple = new Tuple<Login, IEnumerable<UserDetail>>(login.First(), searchresult);
            return View(tuple);

        }
        public ActionResult Search(string search)
        { 
            IEnumerable<Login> login = from t in db.Login where t.UserId == userid select t;
            IEnumerable<UserDetail> searchresult = from s in db.UserDetail where (s.Name.StartsWith(search) || s.Surname.StartsWith(search) || s.Username.StartsWith(search)) && (s.Type == 2 || s.Type == 3 ) select s;
            var tuple = new Tuple<Login, IEnumerable<UserDetail>>(login.First(), searchresult);
            return View(tuple);
            
        }

        public ActionResult AddPerson(int? add)
        {
            IEnumerable<RequestContactList> req = from s in db.RequestContactList where s.OwnerId == userid && s.ContactId == (int)add select s;
            try { if (req.First() == null && add != null)
                {
                    TempData["requestmessage"] = "You have already sent request. ";
                    return Redirect(Request.Url.ToString() );
                } }
            catch {
                using (DiabetEntities db = new DiabetEntities())

                {

                    RequestContactList newrequest = new RequestContactList();
                    newrequest.OwnerId = (int)userid;
                    newrequest.status = 0;
                    newrequest.ContactId = (int)add;
                    db.RequestContactList.Add(newrequest);
                    db.SaveChanges();
                    return Redirect(Request.UrlReferrer.ToString());
                }
                
                
            }
        
          
            return Redirect(Request.UrlReferrer.ToString());
           

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
        
        public ActionResult Messages(int id)
        {

            //int si = 8;
            //id = 6;
                
               
            IEnumerable<Message> messages = from s in db.Message where s.UserId == id && s.SenderId == (int)userid || s.SenderId == id && s.UserId == (int)userid  select s;
            if (messages.Count() >=1)
            { messages.ToList().ForEach(u => { u.IsRead = 1; });
                db.SaveChanges();

                return PartialView(messages);
            }



            else
            {
                using (DiabetEntities db = new DiabetEntities())
                {
                    Message newmessage = new Message();
                    newmessage.UserId = (int)userid;
                    newmessage.SenderId = (int)id;
                    newmessage.MessageLength = "You are now connected. You can write a message";
                    newmessage.MessageDate = DateTime.Now;
                    newmessage.IsRead = 0;
                    db.Message.Add(newmessage);

                    db.SaveChanges();
                }
                IEnumerable<Message> messages2 = from s in db.Message where s.UserId == id && s.SenderId == (int)userid || s.SenderId == id && s.UserId == (int)userid select s;
                return PartialView(messages2);
            }
        }
        public ActionResult AddMessage(int id1, int id2, string typedmessage)
        {
            using (DiabetEntities db = new DiabetEntities()) {
                Message newmessage = new Message();
                
                if (userid == id1)
                { newmessage.UserId = (int)id1;
                    newmessage.SenderId = (int)id2;
                }
                else if (userid == id2)
                { newmessage.UserId = (int)id2; newmessage.SenderId = (int)id1; }
                else
                { return RedirectToAction("Index", "Home"); }



                if (typedmessage.Length < 400 && typedmessage.Length > 1)
                { newmessage.MessageLength = typedmessage; }
                else
                {
                    return Redirect(Request.UrlReferrer.ToString());
                    
                }
                
                newmessage.MessageDate = DateTime.Now;
                newmessage.IsRead = 0;
                db.Message.Add(newmessage);
                
                    db.SaveChanges();
               
                

            }

            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult ContactList(int id )
        {   IEnumerable<ContactList> contactList = from c in db.ContactList where c.OwnerId == id select c;

            return PartialView(contactList);
        }

        public ActionResult AddBloodSugarLevel(int? level )
        {
            using (DiabetEntities db = new DiabetEntities())
            {
                AddBloodSugar bsl = new AddBloodSugar();
                bsl.UserId = (int) userid;
                if (level != null)
                {
                    bsl.BloodSugarValue = (int)level;
                }
                else { return Redirect(Request.UrlReferrer.ToString()); }
                bsl.BloodSugarEntranceDate = DateTime.Now;
                db.AddBloodSugar.Add(bsl);
                db.SaveChanges();
                
            }

            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult DietList(int id)
        {
            dietlistid = id;
            IEnumerable<DietList> a = from c in  db.DietList where c.DietListID == id select c;
            
            return PartialView(a); 
        }
        public ActionResult DeleteDietList ( )
        {

            DietList diet = db.DietList.Find(dietlistid);
            if(diet == null)
            {
                return HttpNotFound();

            }
            diet.GiveToId = 9;
            diet.GivenFromId = 9;

            db.SaveChanges();
            
            
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Profile1()
        {
            if(GetCookie("userid") == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int val = int.Parse(GetCookie("userid"));
            UserDetail userDetail = db.UserDetail.Find(val);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);

        }
        public string ComputeHash(string input)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            SHA256 mySHA256 = SHA256Managed.Create();

            Byte[] hashedBytes = mySHA256.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }
        [HttpPost]
        public ActionResult UpdatePassword(string password)
        {
            if (GetCookie("userid") == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int val = int.Parse(GetCookie("userid"));
            UserDetail userDetail = db.UserDetail.Find(val);
            string hashpassword=  ComputeHash(password);
             
            userDetail.Password = hashpassword; 
            try
            {
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Profile1", "Home");

        }
        [HttpPost]
        public ActionResult UpdateMail(string mail)
        {
            if (GetCookie("userid") == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int val = int.Parse(GetCookie("userid"));
            UserDetail userDetail = db.UserDetail.Find(val);
            userDetail.Mail = mail;
            try
            {
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Profile1", "Home");

        }
        public ActionResult UpdateWeight(string weight )
        {
            if (GetCookie("userid") == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int val = int.Parse(GetCookie("userid"));
            UserDetail userDetail = db.UserDetail.Find(val);
            userDetail.Weight =weight;
            float bmi = (float.Parse(weight)) / (float.Parse(userDetail.Height) * float.Parse(userDetail.Height)) * 10000;
            userDetail.BMI = bmi;
            try
            {
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Profile1", "Home");
        }
        public ActionResult UpdatePP(HttpPostedFileBase file)
        {try { 
            string path1 = "";
            string extension2 = "";
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                extension2 = extension;
                
                Random rand = new Random(DateTime.Now.Millisecond);
                int fname = rand.Next();

                var path = Path.Combine(Server.MapPath("~/Files"), fname.ToString() + extension2);
                file.SaveAs(path);
                path1 = path;

            }
            if (GetCookie("userid") == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int val = int.Parse(GetCookie("userid"));
            UserDetail userDetail = db.UserDetail.Find(val);
            userDetail.ProfilePictureURL = "/Files/" + Path.GetFileName(path1);
            
           
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Profile1", "Home");
            

        }
        public ActionResult Details()
        {if (GetCookie("userid") != null)
            {
                int uid = int.Parse(GetCookie("userid"));
                IEnumerable<AddBloodSugar> bs = from a in db.AddBloodSugar where a.UserId == uid orderby a.BloodSugarEntranceDate descending select a;
                return View(bs);
            }
            else
                return View();
        }
        public ActionResult PopularUsers()
        {
            IEnumerable<Popular> populars = from p in db.Popular orderby  p.Count descending select  p; 

            IEnumerable < UserDetail > userDetails = from l in db.UserDetail where (l.UserId == populars.FirstOrDefault().GivenFromId )select l;

            IEnumerable<Login> login = from t in db.Login where t.UserId == userid select t;
            var tuple = new Tuple<Login, IEnumerable<UserDetail>>(login.First(), userDetails);
            return View(tuple);


           
        }

        
    }
}