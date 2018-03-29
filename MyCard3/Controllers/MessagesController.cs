using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyCard3.Models;

namespace MyCard3.Controllers
{
    public class MessagesController : Controller
    {
        private MyCardContainer db = new MyCardContainer();

        // GET: Messages
        public ActionResult Index(int id = 0)
        {
            Person me = Session["CurrentUserData"] as Person;
            var messages = db.Messages.Where(m => ((m.SendPersonId == id) && (m.ReceivePerson.Id == me.Id)) || ((m.SendPersonId == me.Id) && (m.ReceivePerson.Id == id)));
            ViewBag.SendMessageToID = id;
            return View(messages.ToList());
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create(int id = 0)
        {
            Person friend = db.People.Where(p => p.Id == id).FirstOrDefault();
            TempData["ToFriend"] = friend;
            return View(new Message() { ReceivePerson = friend });
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SendPersonId,MessageContent,Time")] Message message)
        {
            if (ModelState.IsValid)
            {
                Person me = Session["CurrentUserData"] as Person;
                message.SendPersonId = me.Id;
                message.SendPerson = db.People.Where(p => p.Id == me.Id).FirstOrDefault();
                Person friend = TempData["ToFriend"] as Person;
                message.ReceivePerson = db.People.Where(p => p.Id == friend.Id).FirstOrDefault();
                message.Time = DateTime.Now;
                db.Messages.Add(message);
                db.NotificationSet.Add(new Notification { PersonId = friend.Id, Time = DateTime.Now, Content = $"You got a new message from {me.Name}" });
                db.SaveChanges();
                return RedirectToAction("Index",new { id= friend.Id});
            }

            ViewBag.SendPersonId = new SelectList(db.People, "Id", "Name", message.SendPersonId);
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.SendPersonId = new SelectList(db.People, "Id", "Name", message.SendPersonId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SendPersonId,MessageContent,Time")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SendPersonId = new SelectList(db.People, "Id", "Name", message.SendPersonId);
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
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
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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
