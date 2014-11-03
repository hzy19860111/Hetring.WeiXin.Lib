using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXin.Lib.Core.Helper;
using WeiXin.Lib.Core.Models;

namespace Mvc_Demo.Controllers
{
    public class UserInfoController : Controller
    {
        //
        // GET: /UserInfo/

        public ActionResult Index()
        {
            #region 应用授权作用域，snsapi_base

            //授权获取到的code
            string code = "";
            //通过code获取用户信息 不需要关注公众号
            WeiXinUserSampleInfo sampleInfo = WeiXinHelper.GetUserSampleInfo(code);
            string openId = sampleInfo.OpenId;

            #endregion

            #region (思路一致，开发过程中没用到，暂时放空。。)snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）

            //授权获取到的code
            string code2 = "";
            //通过code获取用户信息 不需要关注公众号
            WeiXinUserSampleInfo sampleInfo2 = WeiXinHelper.GetUserSampleInfo(code2);
            string openId2 = sampleInfo2.OpenId;
            string accessToken = sampleInfo2.Access_Token;

            //todo:刷新accessToken 获取 refresh_token（如果需要）

            //todo:获取用户信息


            #endregion

            //这种方法获取用户信息，需要用户关注公众号
            WeiXinUserInfo info = WeiXinHelper.GetUserInfo(openId);

            return View();
        }

    }
}
