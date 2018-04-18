using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyCard3.App_Code;
using MyCard3.Models;
using Newtonsoft.Json;

namespace MyCard3.Controllers
{
    [RequireHttps]
    public class PeopleController : Controller
    {
        private MyCardContainer db = new MyCardContainer();


        //public ActionResult DeleteNotificationCookie()
        //{
        //    SetReadLastNotificationCookie.DeleteCookie(this);
        //    return Content("Hello");
        //}

        public ActionResult GetMyNotifications()
        {
            Person currentUser = Session["CurrentUserData"] as Person;
            var myNotifications = db.NotificationSet.Where(n => n.PersonId == currentUser.Id).OrderByDescending(n => n.Time).Select(n => new { Content = n.Content, Time = n.Time }); //???order???
            //<a class="dropdown-menu" href="#">A</a>
            var json = JsonConvert.SerializeObject(myNotifications);
            //return Json(json,JsonRequestBehavior.AllowGet);
            //SetReadLastNotificationCookie.SetCookie(this);
            if (true == db.People.Where(p => p.Id == currentUser.Id).FirstOrDefault().HasNewNotification)
            {
                db.People.Where(p => p.Id == currentUser.Id).FirstOrDefault().HasNewNotification = false;
                currentUser.HasNewNotification = false;
                db.SaveChanges();
            }
            //Session["HasNewNotification"] = false;

            Session["CurrentUserData"] = currentUser;
            return Content(json);
        }

        public ActionResult HasNewNotifications()
        {
            Person currentUser = Session["CurrentUserData"] as Person;
            var myNotifications = db.NotificationSet.Where(n => n.PersonId == currentUser.Id).OrderByDescending(n => n.Time).Select(n => new { Content = n.Content, Time = n.Time }); //???order???
            //<a class="dropdown-menu" href="#">A</a>
            var json = JsonConvert.SerializeObject(myNotifications);
            //return Json(json,JsonRequestBehavior.AllowGet);
            //SetReadLastNotificationCookie.SetCookie(this);
            return Content(json);
        }

        // GET: People
        public ActionResult Card(int Id = 0)
        {
            Person currentUser = Session["CurrentUserData"] as Person;
            if (Id == 0)
            {
                return View(db.People.AsNoTracking().Where(p => p.Id == currentUser.Id).FirstOrDefault());
            }
            else
            {
                if (db.People.Where(p => p.Id == currentUser.Id).FirstOrDefault().Person2.Select(p => p.Id).Contains(Id))
                {
                    return View(db.People.AsNoTracking().Where(p => p.Id == Id).FirstOrDefault());
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        //public ActionResult Card(int FriendId)
        //{
        //    string tmp = User.Identity.GetUserId();
        //    return View(db.People.AsNoTracking().Where(p => p.Id == FriendId).FirstOrDefault());
        //}

        public ActionResult PhotoUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                ///string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Content/Images/"), "Photo" + User.Identity.GetUserId() + ".png");
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    file.InputStream.CopyTo(ms);
                //    byte[] array = ms.GetBuffer();
                //}

            }
            // after successfully uploading redirect the user
            return RedirectToAction("Card", "People");
        }

        public ActionResult MakeFriend()
        {
            Person currentUser = Session["CurrentUserData"] as Person;
            int partnerId = db.Matches.Where(p => p.A_ID == currentUser.Id).FirstOrDefault().B_ID;
            return View("Card", db.People.Where(p => p.Id == partnerId).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult MakeFriend(string message = "")
        {
            Person currentUser = Session["CurrentUserData"] as Person;
            int partnerId = db.Matches.Where(p => p.A_ID == currentUser.Id).FirstOrDefault().B_ID;
            Person me = db.People.Where(p => p.Id == currentUser.Id).FirstOrDefault();
            Person friend = db.People.Where(p => p.Id == partnerId).FirstOrDefault();
            db.Matches.Where(p => p.A_ID == currentUser.Id).FirstOrDefault().A_OK = true;
            db.Matches.Where(p => p.A_ID == friend.Id).FirstOrDefault().B_OK = true;
            if (true == db.Matches.Where(p => p.A_ID == currentUser.Id).FirstOrDefault().B_OK)
            {
                me.Person1.Add(friend);
                friend.Person1.Add(me);
                db.NotificationSet.Add(new Notification { PersonId = me.Id, Time = DateTime.Now.ToLocalTime(), Content = $"You got a new friend! {friend.Name}" });
                db.NotificationSet.Add(new Notification { PersonId = friend.Id, Time = DateTime.Now.ToLocalTime(), Content = $"You got a new friend! {me.Name}" });
                db.People.Where(p => p.Id == me.Id).FirstOrDefault().HasNewNotification = true;
                db.People.Where(p => p.Id == friend.Id).FirstOrDefault().HasNewNotification = true;
            }
            db.SaveChanges();
            return View("Card", db.People.Where(p => p.Id == partnerId).FirstOrDefault());
        }

        public ActionResult ListFriends()
        {
            Person currentUser = Session["CurrentUserData"] as Person;
            Person me = db.People.Where(p => p.Id == currentUser.Id).FirstOrDefault();
            return View(me.Person2.AsQueryable().Include(p=>p.ReceiveMessage).Include(p=>p.SendMessage));/////
        }

        public ActionResult Index()
        {
            return View(db.People.ToList());
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Gender,Birthday,Mail,Description,Picture")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Gender,Birthday,Mail,Description,Picture")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
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
