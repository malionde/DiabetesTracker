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

namespace MVCExample.font
{
    
    public class LoginController : Controller
    {
        private static string key;
        // GET: Login
        public ActionResult Index()
        {
            string coockie = GetCookie("userid");
            //if (coockie != null) { 
            //    using (DiabetEntities db = new DiabetEntities())
            //    {

            //        var context = db.Login.Where(s => s.Username ==coockie );
            //        var emailReq = context.FirstOrDefault<Login>();
            //        if (emailReq != null && emailReq.TypeApproval != 0)
            //        {
            //            if (emailReq.TypeApproval == 1)
            //            {
            //                return RedirectToAction("Index", "Home", new { id = coockie });
            //            }
            //            else if (emailReq.TypeApproval == 2 || emailReq.TypeApproval == 3)
            //            {
            //                return RedirectToAction("Index", "Experts", new { id = coockie });
            //            }
            //        }
            //        else
            //            return View();

            //    }
            //}

            return View();
        }

        [HttpPost]
        public ActionResult Index(Login model, string Command)
        {
            
            if (Command == "Giris")
            {
                try
                {

                    using (DiabetEntities db = new DiabetEntities())
                    {
                        string hashpassword = ComputeHash(model.Password);
                       
                        var context = db.Login.Where(s => s.Username == model.Username && s.Password == hashpassword);

                        var emailReq = context.FirstOrDefault<Login>();
                        var id = emailReq.UserId;
                        var approval = emailReq.TypeApproval;

                        if (emailReq != null && approval != 0 )
                        {
                            int? id2 = id;

                            HttpCookie cookie = new HttpCookie("userid", id2.ToString());
                            cookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(cookie);
                            if (approval == 1)
                            {
                                return RedirectToAction("Index", "Home", new { id });
                            }
                            else if (approval == 2 ||approval == 3  )
                            {
                                return RedirectToAction("Index", "Experts", new { id });
                            }
                            else if (approval ==4)
                            {
                                return RedirectToAction("Index", "Admin", new { id});
                            }
                        }
                        else if (emailReq == null)
                        {
                            // "Kullanıcı adı ve şifre hatalıdır" diye gösterilecek
                            TempData["message"] = "Username or Password is not correct";
                            return View();
                        }
                        else
                        {
                            TempData["message"] = "Wait for the approval. ";
                            return View();
                        }


                    }
                }
                catch (Exception)
                {
                    TempData["message"] = "Username or Password is not correct";

                }
            }
            if (Command == "KayitOl")
            {
                return RedirectToAction(controllerName: "Login", actionName: "Type");
            }

            return View();

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
        public string ComputeHash(string input)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            
            SHA256 mySHA256 = SHA256Managed.Create();

            Byte[] hashedBytes = mySHA256.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }
        public ActionResult Type()
        {
            return View();
        }

        public ActionResult Recover(string id)
        {
            key = id;
            return View();
        }
        [HttpPost]
        public ActionResult Recover(MVCExample.Models.Login login)
        {

            long key2 = long.Parse(key);
            using (DiabetEntities db = new DiabetEntities())
            {
              
                RecoverAccount2 usermail = db.RecoverAccount2.Find(key2);
                string hashedpassword=  ComputeHash(login.Password);
                usermail.UserDetail.Password = hashedpassword;
                
                //IEnumerable <string> users = from s in db.UserDetail where s.Mail == usermail.Email.First().ToString() select s.Username.ToString();
                //IEnumerable<Login> logins = from l in db.Login where l.Username == users.First() select l;
                //int idd =( int) logins.First().UserId;
                //Login diet = db.Login.Find(idd);
                //diet.Password = login.Password;
                db.SaveChanges();
                return RedirectToAction("Index","Login");
                

                
            }
        }

        public ActionResult RegisterDietitian()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (DiabetEntities context = new DiabetEntities())
            {
                List<Parameter> lstP = context.Parameter.ToList();
                foreach (Parameter item in lstP)
                {

                    SelectListItem sItem = new SelectListItem() { Text = item.GroupName, Value = item.GroupCode.ToString() };

                    lst.Add(sItem);

                }

                ViewBag.GetGender = new SelectList((List<SelectListItem>)lst, "Value", "Text");
              

                return View();
            }
        }
        public ActionResult RegisterPatient()
        {
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
                
                
                SelectListItem sItem2 = new SelectListItem() { Text = "Yes" , Value = "Yes" };
                SelectListItem sItem3 = new SelectListItem() { Text = "No", Value = "No" };
                lst2.Add(sItem2);
                lst2.Add(sItem3);
                ViewBag.GetGender = new SelectList((List<SelectListItem>)lst, "Value", "Text");
                ViewBag.GetInsulin = new SelectList((List<SelectListItem>)lst2, "Value", "Text");
                return View();
            }
        }
        public ActionResult LogOut()
        {
            if (Request.Cookies["userid"] != null)
            {
                var c = new HttpCookie("userid");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult RegisterPatient(UserDetail  model, DateTime ?  dtime, HttpPostedFileBase file)
        {
            string path1 = "";
            string extension2 = "";
            try { 
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                extension2 = extension;
                Random rand = new Random(DateTime.Now.Millisecond);
                int fname = rand.Next();
                
                var path = Path.Combine(Server.MapPath("~/Files"), fname.ToString()+ extension2);
                file.SaveAs(path);
                path1 = path;
                
            }
            
            
            else { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            
            
            string typpeeid = RouteData.Values["id"].ToString();
            }
            catch
            {
                TempData["errormessage"] = "Check your file. It must your approval document.";
            }
            //string hPassword = ComputeHash(model.Password, new SHA256CryptoServiceProvider());
            using (DiabetEntities db = new DiabetEntities())
            {

                try
                {
                    UserDetail _userDetail = new UserDetail();
                    string hashpassword = ComputeHash(model.Password);
                    _userDetail.Password = hashpassword;
                    _userDetail.Name = model.Name;
                    _userDetail.Username = model.Username;
                    _userDetail.Surname = model.Surname;
                    _userDetail.Mail = model.Mail;
                    _userDetail.PhoneNumber = model.PhoneNumber;

                    _userDetail.Weight = model.Weight;
                    _userDetail.Height = model.Height;
                    _userDetail.Gender = model.Gender;
                    _userDetail.BirthDate2 = dtime;
                    _userDetail.InsulinUsage = model.InsulinUsage;
                    _userDetail.ProfilePictureURL = "/Content/dist/img/avatar.png";
                    _userDetail.FileLink = "/Files/" + Path.GetFileName(path1);
                    _userDetail.Type = 1;
                    float bmi = (float.Parse(model.Weight)) / (float.Parse(model.Height) * float.Parse(model.Height)) * 10000;
                    _userDetail.BMI = bmi;
                    _userDetail.PreviousDisease = model.PreviousDisease;
                    db.UserDetail.Add(_userDetail);
                    db.SaveChanges();
                    int? id = _userDetail.UserId;
                    Login _login = new Login();
                    _login.Username = model.Username;
                    _login.Password = hashpassword;
                    _login.UserId = id;
                    _login.TypeApproval = 0;

                    db.Entry(_login).State = EntityState.Added;

                    db.SaveChanges();
                }
                catch(Exception)
                {
                    TempData["errormessage"] = "Check inputs.";
                }


            }

            List<SelectListItem> lst = new List<SelectListItem>();
            using (DiabetEntities context = new DiabetEntities())
            {
                List<Parameter> lstP = context.Parameter.ToList();
                foreach (Parameter item in lstP)
                {

                    SelectListItem sItem = new SelectListItem() { Text = item.GroupName, Value = item.GroupCode.ToString() };

                    lst.Add(sItem);

                }

                ViewBag.GetGender = new SelectList((List<SelectListItem>)lst, "Value", "Text");

                return RedirectToAction("RegisterPatient");
            }
        }
        [HttpPost]
        public ActionResult RegisterDietitian(UserDetail model, HttpPostedFileBase file)
        {
            string path1 = "";
            string extension2 = "";
            try
            {
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
            }
            catch { }
            
            
            //string hPassword = ComputeHash(model.Password, new SHA256CryptoServiceProvider());
           
            using (DiabetEntities db = new DiabetEntities())
            {
                try
                {
                    string typpeeid = RouteData.Values["id"].ToString();
                    UserDetail _userDetail = new UserDetail();
                    _userDetail.Type = int.Parse(typpeeid);
                    string hashpassword = ComputeHash(model.Password);
                    _userDetail.Password = hashpassword;
                    _userDetail.Name = model.Name;
                    _userDetail.Username = model.Username;
                    _userDetail.Surname = model.Surname;
                    _userDetail.Mail = model.Mail;
                    _userDetail.PhoneNumber = model.PhoneNumber;
                    _userDetail.ShortCV = model.ShortCV;

                    _userDetail.ProfilePictureURL = "/Content/dist/img/avatar.png";
                    _userDetail.FileLink = "/Files/" + Path.GetFileName(path1);

                    db.UserDetail.Add(_userDetail);

                    db.SaveChanges();
                    int? id = _userDetail.UserId;
                    Login _login = new Login();
                    _login.Username = model.Username;
                    _login.Password = hashpassword;
                    _login.UserId = id;
                    _login.TypeApproval = 0;
                    db.Entry(_login).State = EntityState.Added;

                    db.SaveChanges();

                }
                catch (Exception)
                {
                    TempData["errormessage"] = "Check inputs.";
                }
            }

            List<SelectListItem> lst = new List<SelectListItem>();
            using (DiabetEntities context = new DiabetEntities())
            {
                List<Parameter> lstP = context.Parameter.ToList();
                foreach (Parameter item in lstP)
                {

                    SelectListItem sItem = new SelectListItem() { Text = item.GroupName, Value = item.GroupCode.ToString() };

                    lst.Add(sItem);

                }

                ViewBag.GetGender = new SelectList((List<SelectListItem>)lst, "Value", "Text");

                return RedirectToAction("RegisterDietitian");
            }
            

        }

        


    }
}