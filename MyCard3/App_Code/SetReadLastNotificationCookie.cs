using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCard3.App_Code
{
    public  static class SetReadLastNotificationCookie
    {

        public static void SetCookie(Controller controller)
        {
            if (controller.Request.Cookies["LastLoginDate"] != null)
            {
                HttpCookie lastLoginDateCookie = controller.Request.Cookies["LastLoginDate"];
                controller.Session["LastLoginDate"] = (DateTime.Parse(lastLoginDateCookie.Value)).ToString("s");
            }
            else
            {
                controller.Session["LastLoginDate"] = DateTime.Now.ToString("s");
            }
            HttpCookie aCookie = new HttpCookie("LastLoginDate");
            aCookie.Value = DateTime.Now.ToString("s");
            aCookie.Expires = DateTime.Now.AddDays(10);
            controller.Response.Cookies.Add(aCookie);
        }
    }
}