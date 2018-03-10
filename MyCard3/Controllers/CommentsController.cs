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
    public class CommentsController : Controller
    {
        private MyCardContainer db = new MyCardContainer();

        // GET: Comments
        public ActionResult CommentList(int articleId = 1)
        {
            var commentSet = db.CommentSet.Where(c=>c.ArticleId== articleId).AsNoTracking();
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

        //// GET: Comments/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ArticleId = new SelectList(db.ArticleSet, "Id", "Title");
        //    ViewBag.PersonId = new SelectList(db.People, "Id", "Name");
        //    return View();
        //}

        //// POST: Comments/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Opinion,ArticleId,PersonId")] Comment comment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CommentSet.Add(comment);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ArticleId = new SelectList(db.ArticleSet, "Id", "Title", comment.ArticleId);
        //    ViewBag.PersonId = new SelectList(db.People, "Id", "Name", comment.PersonId);
        //    return View(comment);
        //}

        //// GET: Comments/Edit/5
        //public ActionResult Edit(int? id)
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
        //    ViewBag.ArticleId = new SelectList(db.ArticleSet, "Id", "Title", comment.ArticleId);
        //    ViewBag.PersonId = new SelectList(db.People, "Id", "Name", comment.PersonId);
        //    return View(comment);
        //}

        //// POST: Comments/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Opinion,ArticleId,PersonId")] Comment comment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(comment).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ArticleId = new SelectList(db.ArticleSet, "Id", "Title", comment.ArticleId);
        //    ViewBag.PersonId = new SelectList(db.People, "Id", "Name", comment.PersonId);
        //    return View(comment);
        //}

        //// GET: Comments/Delete/5
        //public ActionResult Delete(int? id)
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
