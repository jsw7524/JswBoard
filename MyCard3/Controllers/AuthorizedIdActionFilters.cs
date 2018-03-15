using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyCard3.Models;

namespace MyCard3.Controllers
{
    public class AuthorizedIdActionFilters: FilterAttribute, IActionFilter 
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int? articleId = filterContext.ActionParameters["id"] as int?;
            MyCardContainer db = new MyCardContainer();

            Article article =db.ArticleSet.Find(articleId);
            string pMail = (db.People.Where(p => p.Id == article.PersonId).FirstOrDefault()).authenticationId;

            if (pMail!=filterContext.HttpContext.User.Identity.Name)
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }
    }
}