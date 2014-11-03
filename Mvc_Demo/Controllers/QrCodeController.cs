using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXin.Lib.Core.Helper;

namespace Mvc_Demo.Controllers
{
    public class QrCodeController : Controller
    {
        //
        // GET: /QrCode/

        public ActionResult Index()
        {
            string scene_id = "1";//场景Id，扫描生产的二维码 会带这个id，可以做推广、统计用

            //获取二维码流
            System.IO.Stream stream = WeiXinHelper.GetQrCodeImageStream(scene_id);

            //二维码图片存储路径
            string path = Server.MapPath("~/Document/QrCode/" + scene_id + ".jpg"); 
            //直接保存二维码到本地
            WeiXinHelper.SaveQrCodeImage("1", path);

            ViewBag.Path = Url.Content("~/Document/QrCode/" + scene_id + ".jpg");
            return View();
        }

    }
}
