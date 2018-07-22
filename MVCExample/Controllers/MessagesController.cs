using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCExample.Models;

namespace MVCExample.Controllers
{
    public class MessagesController : Controller
    {
        private DiabetEntities db = new DiabetEntities();

        // GET: Messages
        public ActionResult Index()
        {
            var message = db.Message.Include(m => m.UserDetail).Include(m => m.UserDetail1);
            return View(message.ToList());
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            ViewBag.SenderId = new SelectList(db.UserDetail, "UserId", "Name");
            ViewBag.UserId = new SelectList(db.UserDetail, "UserId", "Name");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,UserId,MessageDate,MessageLength,SenderId")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Message.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SenderId = new SelectList(db.UserDetail, "UserId", "Name", message.SenderId);
            ViewBag.UserId = new SelectList(db.UserDetail, "UserId", "Name", message.UserId);
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.SenderId = new SelectList(db.UserDetail, "UserId", "Name", message.SenderId);
            ViewBag.UserId = new SelectList(db.UserDetail, "UserId", "Name", message.UserId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,UserId,MessageDate,MessageLength,SenderId")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SenderId = new SelectList(db.UserDetail, "UserId", "Name", message.SenderId);
            ViewBag.UserId = new SelectList(db.UserDetail, "UserId", "Name", message.UserId);
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Message.Find(id);
            db.Message.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
