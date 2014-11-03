using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXin.Lib.Core.Helper;

namespace Mvc_Demo.Controllers
{
    public class GeoCoderController : Controller
    {
        //
        // GET: /GeoCoder/

        public ActionResult Index()
        {
            //需配置 WeiXineConst的BaiduAk
            string lat = "31.1430"; //经度
            string lng = "121.2943";// 纬度
            var model = WeiXinHelper.GeoCoder(lat, lng);
            return View(model);
        }

    }
}
