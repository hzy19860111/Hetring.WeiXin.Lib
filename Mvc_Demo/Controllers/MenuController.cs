using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXin.Lib.Core.Helper;
using WeiXin.Lib.Core.Models.Message;

namespace Mvc_Demo.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/


        public ActionResult Create()
        {
            string menuFile = Server.MapPath("~/Document/menu.txt");
            string menuString = System.IO.File.ReadAllText(menuFile);

            ErrorMessage message = WeiXinHelper.CreateMenu(menuString);

            return View(message);
        }

        public ActionResult Select()
        {
            string menuString =WeiXinHelper.GetMenu();

            Response.Write(menuString);

            return View();
        }

        public ActionResult Delete()
        {
            bool result = WeiXinHelper.DeleteMenu();
            return View(result);
        }



    }
}
