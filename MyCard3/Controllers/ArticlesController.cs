using System;
using System.Collections;
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
    public class ArticlesController : Controller
    {
        private MyCardContainer db = new MyCardContainer();

        [HttpPost, ValidateInput(false)]
        public void EditorTest(string editordata)
        {
            int i = 1;
        }

        public ActionResult HotArticles()
        {
            return PartialView(db.ArticleSet.OrderByDescending(a => a.ThumbUpNumber).Take(10));
        }

        [HttpPost]
        public ActionResult ThumberUp(int articleId)
        {
            int i = 1;
            Person currentUser = Session["CurrentUserData"] as Person;
            Article article = db.ArticleSet.Where(a => a.Id == articleId).FirstOrDefault();
            if (!db.ArticleThumberUpSet.Where(tu => (tu.PersonId == currentUser.Id) && (tu.ArticleId == articleId)).Any())
            {
                db.ArticleThumberUpSet.Add(new ArticleThumberUp { PersonId = currentUser.Id, ArticleId = articleId });
                article.ThumbUpNumber += 1;
                int tmpN = 0;
                switch (article.ThumbUpNumber)
                {
                    case 10:
                        tmpN = 10;
                        break;
                    case 5:
                        tmpN = 5;
                        break;
                    case 1:
                        tmpN = 1;
                        break;
                }
                db.NotificationSet.Add(new Notification { PersonId = article.PersonId, Time = DateTime.Now.ToLocalTime(), Content = $"You got {tmpN} ThumbUp(s) in {article.Title}" });
                db.People.Where(p => p.Id == currentUser.Id).FirstOrDefault().HasNewNotification = true;
                db.SaveChanges();
            }
            return Json(new { n = article.ThumbUpNumber });
        }

        // GET: Articles
        [Authorize]
        public ActionResult Index(int? boardId = 1)
        {
            ////////////// order articles
            var startDate = DateTime.Now.ToLocalTime().AddMonths(-3);
            var articleSet = db.ArticleSet.AsNoTracking().Where(a => a.BoardId == boardId)
                .Where(a =>  (!a.IsHidden && a.Time > startDate) || a.IsOnTop ).ToList();
            articleSet = articleSet.OrderByDescending(a => a.IsOnTop).ThenByDescending(a => a.Time).ToList();
            //////////////
            ViewData["BoardName"] = articleSet.FirstOrDefault().Board.Name;
            ViewData["BoardId"] = articleSet.FirstOrDefault().Board.Id;
            return View(articleSet);
        }

        // GET: Articles/Details/5
        [Authorize]
        [ArticleAuthorAuthorizeAttribute(Roles = "BoardAdmin,ConfirmedUser,ArticleOwner")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Article article = db.ArticleSet.Find(id);
            Article article = db.ArticleSet.AsNoTracking().Where(a => a.Id == id).Include(c => c.Comment).FirstOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        [Authorize(Roles = "BoardAdmin,ConfirmedUser")]
        public ActionResult Create(int boardId = 1)
        {
            //ViewBag.BoardId = new SelectList(db.BoardSet, "Id", "Name");
            //ViewBag.PersonId = new SelectList(db.People, "Id", "Name");
            ViewData["BoardId"] = boardId;
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles = "BoardAdmin,ConfirmedUser")]
        public ActionResult Create([Bind(Include = "Id,Title,Content")] Article article, int boardId = 1)
        {
            if (ModelState.IsValid)
            {
                article.Time = DateTime.Now.ToLocalTime();
                var authenticationId = User.Identity.GetUserId();
                article.PersonId = db.People.Where(p => p.authenticationId == authenticationId).FirstOrDefault().Id;
                article.BoardId = boardId;
                db.ArticleSet.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index", new { boardId = article.BoardId });
            }

            //ViewBag.BoardId = new SelectList(db.BoardSet, "Id", "Name", article.BoardId);
            //ViewBag.PersonId = new SelectList(db.People, "Id", "Name", article.PersonId);
            return View(article);
        }

        // GET: Articles/Edit/5
        [ArticleAuthorAuthorizeAttribute(Roles = "BoardAdmin,ArticleOwner")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.ArticleSet.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.BoardId = new SelectList(db.BoardSet, "Id", "Name", article.BoardId);
            ViewBag.PersonId = new SelectList(db.People, "Id", "Name", article.PersonId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Time,Content,BoardId,PersonId")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BoardId = new SelectList(db.BoardSet, "Id", "Name", article.BoardId);
            ViewBag.PersonId = new SelectList(db.People, "Id", "Name", article.PersonId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.ArticleSet.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.ArticleSet.Find(id);
            db.ArticleSet.Remove(article);
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
