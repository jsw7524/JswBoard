using Microsoft.AspNet.Identity;
using MyCard3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCard3.Controllers
{
    public class CommentAuthorAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            int commentId = int.Parse((string)httpContext.Request.RequestContext.RouteData.Values["id"]);
            Comment comment;
            string authenticationId;
            using (MyCardContainer db = new MyCardContainer())
            {
                comment = db.CommentSet.Find(commentId);
                authenticationId = (db.People.Where(p => p.Id == comment.PersonId).FirstOrDefault()).authenticationId;
            }

            if (authenticationId == httpContext.User.Identity.GetUserId())
            {
                return true;
            }
            return base.AuthorizeCore(httpContext);
        }
    }
}