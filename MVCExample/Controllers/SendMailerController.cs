using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using MVCExample.Models;
namespace MVCExample.Controllers
{
    public class SendMailerController : Controller
    {   
        //  
        // GET: /SendMailer/   
        public ActionResult Index()
        {
            return PartialView();
        }
        [HttpPost]
        public ViewResult Index(MVCExample.Models.MailModel _objModelMail)
        {
            using (DiabetEntities db = new DiabetEntities())
            {
                RecoverAccount2 rcv = new RecoverAccount2();

                IEnumerable<UserDetail> email = from s in db.UserDetail where s.Mail == _objModelMail.To select s;
                if (email != null)
                {
                    if (ModelState.IsValid)
                    {
                        MailMessage mail = new MailMessage();
                        mail.To.Add(_objModelMail.To);
                        mail.From = new MailAddress("diabettracker@gmail.com");
                        mail.Subject = "Account Recovery";
                        string Body = _objModelMail.Body;
                        Random rand = new Random(DateTime.Now.Millisecond);
                        int random = rand.Next();
                        mail.Body = "<a href=\"http://localhost:56178/Login/Recover/"   +   random.ToString()   + "\">Press to recover your account. </a>";
                        rcv.Email = _objModelMail.To;
                        rcv.Key = random;
                        rcv.userid = email.First().UserId;
                        db.RecoverAccount2.Add(rcv);
                        
                        db.SaveChanges();
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("diabettracker@gmail.com", "password"); // Enter seders User name and password   
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                        return View("Index", _objModelMail);
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                    {
                        return View();
                    }
                
            }
        }
    }
}
