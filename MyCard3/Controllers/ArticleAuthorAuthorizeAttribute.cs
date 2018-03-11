using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCard3.Models;
using Microsoft.AspNet.Identity;

namespace MyCard3.Controllers
{
    public class ArticleAuthorAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            int articleId = int.Parse((string)httpContext.Request.RequestContext.RouteData.Values["id"]);
            Article article;
            string pMail ;
            using (MyCardContainer db = new MyCardContainer())
            {
                article = db.ArticleSet.Find(articleId);
                pMail = (db.People.Where(p => p.Id == article.PersonId).FirstOrDefault()).Mail;
            }

            if (pMail == httpContext.User.Identity.Name)
            {
                return true;
            }
            return base.AuthorizeCore(httpContext);
        }
    }
}