using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyCard3.Models;

namespace MyCard3.Controllers
{
    [RequireHttps]
    public class CommentsController : Controller
    {
        private MyCardContainer db = new MyCardContainer();

        [HttpPost]
        public ActionResult ThumberUp(int commentId)
        {
            int i = 1;
            Person currentUser = Session["CurrentUserData"] as Person;
            Comment comment = db.CommentSet.Where(c => c.Id == commentId).Include(c => c.Article).FirstOrDefault();
            if (!db.CommentThumberUpSet.Where(tu => (tu.PersonId == currentUser.Id) && (tu.CommentId == commentId)).Any())
            {
                db.CommentThumberUpSet.Add(new CommentThumberUp { PersonId = currentUser.Id, CommentId = commentId });
                comment.ThumberUpNumber += 1;
                int tmpN = 0;
                switch (comment.ThumberUpNumber)
                {
                    case 100:
                        tmpN = 100;
                        break;
                    case 60:
                        tmpN = 60;
                        break;
                    case 30:
                        tmpN = 30;
                        break;
                    case 15:
                        tmpN = 15;
                        break;
                    case 5:
                        tmpN = 5;
                        break;
                    case 1:
                        tmpN = 1;
                        break;
                }
                db.NotificationSet.Add(new Notification { PersonId = comment.PersonId, Time = DateTime.Now.ToLocalTime(), Content = $"You got {tmpN} ThumbUp(s) in {comment.Article.Title}" });
                db.People.Where(p => p.Id == currentUser.Id).FirstOrDefault().HasNewNotification = true;
                db.SaveChanges();
            }
            return Json(new { n = comment.ThumberUpNumber });
        }

        // GET: Comments
        public ActionResult CommentList(int articleId = 1)
        {
            var commentSet = db.CommentSet.Where(c => c.ArticleId == articleId).AsNoTracking();
            return View(commentSet.ToList());
        }

        //// GET: Comments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Comment comment = db.CommentSet.Find(id);
        //    if (comment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(comment);
        //}

        // GET: Comments/Create
        [Authorize(Roles = "BoardAdmin,ConfirmedUser")]
        public ActionResult Create(int articleId = 1, int personId = 1)
        {
            ViewBag.ArticleId = articleId;
            ViewBag.PersonId = personId;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles = "BoardAdmin,ConfirmedUser")]
        public ActionResult Create([Bind(Include = "Id,Opinion,ArticleId,PersonId")] Comment comment, int articleId = 0)
        {
            if (ModelState.IsValid)
            {
                comment.ArticleId = articleId;
                Person p = Session["CurrentUserData"] as Person;
                comment.PersonId = p.Id;
                db.CommentSet.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Articles", new { Id = articleId });
            }
            return RedirectToAction("Details", "Articles", new { Id = articleId });
        }

        // GET: Comments/Edit/5
        [CommentAuthorAuthorizeAttribute(Roles = "BoardAdmin,ArticleOwner")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.CommentSet.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleId = new SelectList(db.ArticleSet, "Id", "Title", comment.ArticleId);
            ViewBag.PersonId = new SelectList(db.People, "Id", "Name", comment.PersonId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CommentAuthorAuthorizeAttribute(Roles = "BoardAdmin,ArticleOwner")]
        public ActionResult Edit([Bind(Include = "Id,Opinion,ArticleId,PersonId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Articles", new { id = comment.ArticleId });
            }
            ViewBag.ArticleId = new SelectList(db.ArticleSet, "Id", "Title", comment.ArticleId);
            ViewBag.PersonId = new SelectList(db.People, "Id", "Name", comment.PersonId);
            return View(comment);
        }

        //// GET: Comments/Delete/5
        [CommentAuthorAuthorizeAttribute(Roles = "BoardAdmin,ArticleOwner")]
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.CommentSet.Find(Id);

            if (comment == null)
            {
                return HttpNotFound();
            }
            int articleId = comment.ArticleId;

            comment.Opinion = "This Comment has been deleted by user!";
            db.SaveChanges();
            return RedirectToAction("Details", "Articles", new { Id = articleId });
        }

        //// POST: Comments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Comment comment = db.CommentSet.Find(id);
        //    db.CommentSet.Remove(comment);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
