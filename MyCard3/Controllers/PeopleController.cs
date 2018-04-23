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

        public ActionResult MemberList()
        {
            return View(db.People);
        }

        public ActionResult ToggleComfirmedUser(int id)
        {
            var target = db.People.Find(id);
            target.ComfirmedUser = !target.ComfirmedUser;
            db.SaveChanges();
            return Json(target.ComfirmedUser, JsonRequestBehavior.AllowGet);
        }


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
                //if (db.People.Where(p => p.Id == currentUser.Id).FirstOrDefault().Person2.Select(p => p.Id).Contains(Id))
                if (db.Friends.Where(p => p.PersonA.Id == currentUser.Id).Select(p => p.PersonB.Id).Contains(Id))
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
            Person currentUser = Session["CurrentUserData"] as Person;
            return RedirectToAction("Edit", "People",new {Id= 0 });
        }

        public ActionResult IdCardUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Content/IdCards/"), "IdCard" + User.Identity.GetUserId() + ".png");

                file.SaveAs(path);


            }
            Person currentUser = Session["CurrentUserData"] as Person;
            return RedirectToAction("Edit", "People",new { Id= 0});
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
                //me.Person1.Add(friend);
                //friend.Person1.Add(me);
                db.Friends.Add(new Friend() { PersonA = me, PersonB = friend, LastMessage = "" });
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
            //use Eagerly loading to avoid multiple queries (discard)
            //Person me = db.People.Where(p => p.Id == currentUser.Id).Include(p=>p.ReceiveMessage).Include(p=>p.SendMessage).FirstOrDefault();
            //
            Person me = db.People.Where(p => p.Id == currentUser.Id).FirstOrDefault();
            var myFriends = db.Friends.AsEnumerable().Where(f => f.PersonA == me).Select(f => f.PersonB).ToList();
            IDictionary<int, string> lastMessage = db.Friends.AsEnumerable()
                                                        .Select(f => new { key = f.PersonB.Id, LastMessage = f.LastMessage })
                                                        .ToDictionary(f => f.key, f => f.LastMessage);
            return View(new FriendListViewModel() { Friends = myFriends, LastMessage = lastMessage });
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
        public ActionResult Create([Bind(Include = "Id,Name,Gender,Birthday,Description,Picture")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }
        /// <summary>
        /// ////////////////////////
        public ActionResult AdminEdit(int? id)
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
        public ActionResult AdminEdit(Person person, string url)
        {
            if (ModelState.IsValid)
            {
                var user = db.People.Find(person.Id);
                user.Name = person.Name;
                user.Gender = person.Gender;
                user.Description = person.Description;
                user.Birthday = person.Birthday;
                user.ComfirmedUser = person.ComfirmedUser;
                user.Department = person.Department;
                user.ComfirmedUser = person.ComfirmedUser;
                db.SaveChanges();
                return Redirect(url);
            }
            return View(person);
        }
        /// /////////////////////

        // GET: People/Edit/5
        public ActionResult Edit(int? id=0)
        {
            if (id == 0)
            {
                Person currentUser = Session["CurrentUserData"] as Person;
                return View(db.People.AsNoTracking().Where(p => p.Id == currentUser.Id).FirstOrDefault());
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
        public ActionResult Edit(Person person, string url)
        {
            if (ModelState.IsValid)
            {
                Person currentUser = Session["CurrentUserData"] as Person;
                var user = db.People.Find(currentUser.Id);
                user.Name = person.Name;
                user.Gender = person.Gender;
                user.Description = person.Description;
                user.Birthday = person.Birthday;
                user.ComfirmedUser = person.ComfirmedUser;
                user.Department = person.Department;
                db.SaveChanges();
                return Redirect(url);
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
