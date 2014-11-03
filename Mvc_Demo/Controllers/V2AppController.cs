using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXin.Lib.Core.Consts;
using WeiXin.Lib.Core.Helper;
using WeiXin.Lib.Core.Helper.WXPay;

namespace Mvc_Demo.Controllers
{
    public class V2AppController : Controller
    {
        //
        // GET: /V2App/

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// App 预支付
        /// </summary>
        /// <returns></returns>
        public ActionResult AppPrePay()
        {
            string orderNo = ""; //订单编号，文档中的out_trade_no
            string description = ""; //订单详情
            string totalFee = "";//订单金额（单位：分）
            string notifyUrl = ""; //支付通知Url
            string createIp = "";//用户IP

            string traceId = string.Empty; //

            WxPayModel model = WxPayModel.Create(description, orderNo, totalFee, notifyUrl, createIp);

            var result = WeiXinHelper.PreWXPay(model.CreateAppPrePayPackage(traceId));

            if (!string.IsNullOrEmpty(result.PrePayId))
            {
                Response.Write("预支付ID：" + result.PrePayId);
            }
            else
            {
                Response.Write("预支付失败！");
            }

            return View();
        }
    }
}
