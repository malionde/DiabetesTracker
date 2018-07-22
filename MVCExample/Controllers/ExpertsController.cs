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
    public class ExpertsController : Controller
    {
        private DiabetEntities db = new DiabetEntities();
        // GET: 
        private static int? userid = 0;
        private static int dietlistid;
        private static int? AddDietListID; 
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
                    IEnumerable<DietList> dlist = from dl in db.DietList where dl.GivenFromId == (int)userid orderby dl.DietListDate descending  select dl;
                    IEnumerable<Message> msg = from m in db.Message where m.SenderId == id && m.IsRead == 0 select m;
                    int notificiation = msg.Count();
                    ViewBag.notify = notificiation.ToString();
                    var tuple = new Tuple<Login, IEnumerable<RequestContactList>, IEnumerable<ContactList>, IEnumerable<DietList>>(login.First(), requestContactLists, contacts, dlist);


                    return View(tuple);
                }

                else
                    return RedirectToAction("Index", "Experts", new { id = GetCookie("userid") });
            }
            else if (GetCookie("userid") != null)
            {
                return RedirectToAction("Index", "Experts", new { id = GetCookie("userid") });
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult Profile1()
        {
            if (GetCookie("userid") == null)
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
            string hashpassword = ComputeHash(password);

            userDetail.Password = hashpassword;
            try
            {
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Profile1", "Experts");

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
            return RedirectToAction("Profile1", "Experts");

        }
        public ActionResult UpdatePP(HttpPostedFileBase file)
        {
            try
            {
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
            return RedirectToAction("Profile1", "Experts");


        }
        public ActionResult Search(string search)
        {
            IEnumerable<UserDetail> searchresult = from s in db.UserDetail where s.Name.StartsWith(search) || s.Surname.StartsWith(search) || s.Username.StartsWith(search) select s;

            return View(searchresult);

        }
        
        public ActionResult AddDietList()
        {
            IEnumerable<Login> login = from t in db.Login where t.UserId == userid select t;
            IEnumerable<ContactList> contactList = from c in db.ContactList where c.OwnerId == userid select c;
            var tuple = new Tuple<Login, IEnumerable<ContactList>>(login.First(), contactList);
            return View(tuple);
        }
        
        public ActionResult AddDietList2(int ? id )
        {
            //if (add == null)
            //{


            //    return RedirectToAction("AddDietList","Experts");
            //}
            //else
            //{
            IEnumerable<Login> login = from t in db.Login where t.UserId == userid select t;
            AddDietListID = (int) id; 
                List<SelectListItem> lst = new List<SelectListItem>();
                List<SelectListItem> lst2 = new List<SelectListItem>();
                using (DiabetEntities context = new DiabetEntities())
                {
                    List<Parameter> lstP = context.Parameter.ToList();
                    foreach (Parameter item in lstP)
                    {

                        SelectListItem sItem = new SelectListItem() { Text = item.GroupName, Value = item.GroupCode.ToString() };

                        lst.Add(sItem);

                    }

                DiabetEntities db = new DiabetEntities();
                DietList dietList = new DietList();

                SelectListItem sItem2 = new SelectListItem() { Text = "Yes", Value = "Yes" };   
                    SelectListItem sItem3 = new SelectListItem() { Text = "No", Value = "No" };
                    lst2.Add(sItem2);
                    lst2.Add(sItem3);
                    ViewBag.GetGender = new SelectList((List<SelectListItem>)lst, "Value", "Text");
                    ViewBag.GetInsulin = new SelectList((List<SelectListItem>)lst2, "Value", "Text");
                var tuple = new Tuple<Login, DietList>(login.First(), dietList);
                return View(dietList);
                //}
            }

        }
        [HttpPost]
        public ActionResult AddDietList2([Bind(Include = "DietListContent,MaxAge,MinAge,MaxBMI,MinBMI,Gender,InsulingUsage")] DietList model)
        {
            if (AddDietListID == null)
            {

                return RedirectToAction("AddDietList", "Experts");

            }
            if(userid == null )
            {   
                return RedirectToAction("AddDietList", "Experts");
                
            }
            if (model.MaxAge == null)
            {

                return RedirectToAction("AddDietList", "Experts");
            }
            else
            {

                using (DiabetEntities db = new DiabetEntities())
                {

                    DietList dietList = new DietList();
                    dietList.DietListContent = model.DietListContent;
                    dietList.DietListDate = DateTime.Now;
                    dietList.Gender = model.Gender;
                    dietList.GivenFromId = (int)userid;
                    dietList.GiveToId = (int)AddDietListID;
                    dietList.MaxAge = model.MaxAge;
                    dietList.MinAge = model.MinAge;
                    dietList.MinBMI = model.MinBMI;
                    dietList.MaxBMI = model.MaxBMI;
                    dietList.InsulingUsage = model.InsulingUsage;
                    db.DietList.Add(dietList);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Experts",new { id = userid});



                }
            }

        }
        public ActionResult AcceptRequest(int? acceptid )
        {

            if (acceptid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestContactList contact  = db.RequestContactList.Find(acceptid);
            
            if (contact == null)
            {
                return HttpNotFound();
            }
            contact.status = 1;
           
            db.SaveChanges(); 

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


            IEnumerable<Message> messages = from s in db.Message where s.UserId == id && s.SenderId == (int)userid || s.SenderId == id && s.UserId == (int)userid select s;
            if (messages.Count() >= 1)
            {
                messages.ToList().ForEach(u => { u.IsRead = 1; });
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
                    db.Message.Add(newmessage);

                    db.SaveChanges();
                }
                IEnumerable<Message> messages2 = from s in db.Message where s.UserId == id && s.SenderId == (int)userid || s.SenderId == id && s.UserId == (int)userid select s;
                return PartialView(messages2);
            }
        }
        public ActionResult AddMessage(int id1, int id2, string typedmessage)
        {
            using (DiabetEntities db = new DiabetEntities())
            {
                Message newmessage = new Message();

                if (userid == id1)
                {
                    newmessage.UserId = (int)id1;
                    newmessage.SenderId = (int)id2;
                }
                else if (userid == id2)
                { newmessage.UserId = (int)id2; newmessage.SenderId = (int)id1; }
                else
                { return RedirectToAction("Index", "Experts"); }



                if (typedmessage.Length < 400 && typedmessage.Length > 1)
                { newmessage.MessageLength = typedmessage; }
                else
                {
                    return Redirect(Request.UrlReferrer.ToString());

                }

                newmessage.MessageDate = DateTime.Now;
                db.Message.Add(newmessage);
                newmessage.IsRead = 0;
                db.SaveChanges();



            }

            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult ContactList(int id)
        {
            IEnumerable<ContactList> contactList = from c in db.ContactList where c.OwnerId == id select c;

            return PartialView(contactList);
        }

        public ActionResult AddBloodSugarLevel(int? level)
        {
            using (DiabetEntities db = new DiabetEntities())
            {
                AddBloodSugar bsl = new AddBloodSugar();
                bsl.UserId = (int)userid;
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
            IEnumerable<DietList> a = from c in db.DietList where c.DietListID == id select c;

            return PartialView(a);
        }
        public ActionResult DeleteDietList()
        {

            DietList diet = db.DietList.Find(dietlistid);
            if (diet == null)
            {
                return HttpNotFound();

            }
            diet.GiveToId = 9;

            db.SaveChanges();


            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Details (int? id )
        {
            IEnumerable<AddBloodSugar> bs = from a in db.AddBloodSugar where a.UserId == id orderby a.BloodSugarEntranceDate descending select a; 
            return View(bs);
        }
    }
}