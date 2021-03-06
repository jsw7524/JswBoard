﻿using System;
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
    public class BoardsController : Controller
    {
        private MyCardContainer db = new MyCardContainer();

        // GET: Boards
        public ActionResult Index()
        {
            //////////////
            //Session["CurrentUserAuthenticationID"] = User.Identity.GetUserId();
            string tmp = User.Identity.GetUserId();
            Person currentUser = db.People.AsNoTracking().Where(p => p.authenticationId == tmp).AsEnumerable().FirstOrDefault();
            Session["CurrentUserData"] = currentUser;
            //////////////
            //if (currentUser != null)
            //{
            //    if (currentUser.HasNewNotification)
            //    {
            //        Session["HasNewNotification"] = true;
            //    }
            //}
            return View(db.BoardSet.ToList());
        }

        // GET: Boards/Details/5
        [Authorize(Roles = "BoardAdmin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.BoardSet.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // GET: Boards/Create
        [Authorize(Roles = "BoardAdmin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Boards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BoardAdmin")]
        public ActionResult Create([Bind(Include = "Id,Name")] Board board)
        {
            if (ModelState.IsValid)
            {
                db.BoardSet.Add(board);
                db.SaveChanges();
                db.ArticleSet.Add(new Article() { Title = "版規", Time = DateTime.Now.ToLocalTime(), BoardId = board.Id, PersonId = 1 });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(board);
        }

        // GET: Boards/Edit/5
        [Authorize(Roles = "BoardAdmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.BoardSet.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BoardAdmin")]
        public ActionResult Edit([Bind(Include = "Id,Name")] Board board)
        {
            if (ModelState.IsValid)
            {
                db.Entry(board).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(board);
        }

        // GET: Boards/Delete/5
        [Authorize(Roles = "BoardAdmin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.BoardSet.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BoardAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Board board = db.BoardSet.Find(id);
            db.BoardSet.Remove(board);
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
