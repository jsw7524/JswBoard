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
            string authenticationId;
            using (MyCardContainer db = new MyCardContainer())
            {
                article = db.ArticleSet.Find(articleId);
                authenticationId = (db.People.Where(p => p.Id == article.PersonId).FirstOrDefault()).authenticationId;
            }

            if (authenticationId == httpContext.User.Identity.GetUserId())
            {
                return true;
            }
            return base.AuthorizeCore(httpContext);
        }
    }
}