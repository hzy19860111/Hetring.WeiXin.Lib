using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using WeiXin.Lib.Core.Consts;
using WeiXin.Lib.Core.Models;
using WeiXin.Lib.Core.Models.Message;
using WeiXin.Lib.Core.Models.UnifiedMessage;

namespace WeiXin.Lib.Core.Helper
{
    public class WeiXinHelper
    {
        #region 获取AccessToken

        /// <summary>
        /// 获取Access_Token
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken()
        {
            string url = WeiXinConst.WeiXin_AccessTokenUrl;
            string result = HttpClientHelper.GetResponse(url);
            return result;
        }

        #endregion

        #region 获取用户信息

        /// <summary>
        /// 根据用户Code获取用户信息（包括OpenId的简单信息）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WeiXinUserSampleInfo GetUserSampleInfo(string code)
        {
            string url = string.Format(WeiXinConst.WeiXin_User_OpenIdUrl, code);
            WeiXinUserSampleInfo info = HttpClientHelper.GetResponse<WeiXinUserSampleInfo>(url);
            return info;
        }

        /// <summary>
        /// 根据用户Code获取用户信息（包括OpenId的简单信息）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetUserOpenId(string code)
        {
            return GetUserSampleInfo(code).OpenId;
        }

        /// <summary>
        /// 根据OpenID 获取用户基本信息(需关注公众号）
        /// </summary>
        /// <param name="openId"></param>
        public static WeiXinUserInfo GetUserInfo(string openId)
        {
            var token = AccessToken.Instance;
            string url = string.Format(WeiXinConst.WeiXin_User_GetInfoUrl, token.Access_Token, openId);

            string result = HttpClientHelper.GetResponse(url);

            if (string.IsNullOrEmpty(result))
                return null;

            WeiXinUserInfo info = JsonConvert.DeserializeObject<WeiXinUserInfo>(result);
            //解析用户信息失败，判断 失败Code ，40001 为AccessToken失效，重新创建Token并获取用户信息
            if (info == null || string.IsNullOrEmpty(info.OpenId))
            {
                ErrorMessage msg = JsonConvert.DeserializeObject<ErrorMessage>(result);
                if (msg.TokenExpired)
                {
                    return GetUserInfoByNewAccessToken(openId);
                }
            }

            return info;
        }

        /// <summary>
        /// 创建新的AccessToken 并获取用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        private static WeiXinUserInfo GetUserInfoByNewAccessToken(string openId)
        {
            var token = AccessToken.NewInstance;
            string url = string.Format(WeiXinConst.WeiXin_User_GetInfoUrl, token.Access_Token, openId);
            WeiXinUserInfo info = HttpClientHelper.GetResponse<WeiXinUserInfo>(url);
            return info;
        }

        #endregion

        #region 构建OAuth2授权Url

        /// <summary>
        /// 构建为 OAuth2授权 的Url
        /// </summary>
        /// <param name="url">要跳转的Url</param>
        /// <param name="state">状态，可用来记录当前用户状态，或校验是否合法请求</param>
        /// <returns></returns>
        public static string GetOAuth2Url(string url, string state = "")
        {
            url = HttpUtility.UrlEncode(url);

            return string.Format(WeiXinConst.WeiXin_User_OAuth2Url, url, state); ;
        }

        #endregion

        #region 生成二维码

        /// <summary>
        /// 获取Ticket
        /// </summary>
        /// <returns></returns>
        private static string CreateTicket(string scene_id)
        {
            var token = AccessToken.Instance;

            if (string.IsNullOrEmpty(token.Access_Token))
                throw new ArgumentNullException("Access_Token");

            string url = string.Format(WeiXinConst.WeiXin_Ticket_CreateUrl, token.Access_Token);
            string postData = WeiXinConst.WeiXin_QrCodeTicket_Create_JsonString.Replace("{0}", scene_id);

            string result = HttpClientHelper.PostResponse(url, postData);
            Ticket ticket = JsonConvert.DeserializeObject<Ticket>(result); //HttpClientHelper.PostResponse<Ticket>(url, postData);

            if (ticket == null || string.IsNullOrEmpty(ticket.ticket))
            {
                ErrorMessage msg = JsonConvert.DeserializeObject<ErrorMessage>(result);
                if (msg.TokenExpired)
                    return CreateTicketByNewAccessToken(scene_id);
            }
            return ticket.ticket;
        }

        /// <summary>
        /// 当AccessToken过期时 调用此方法
        /// </summary>
        /// <param name="scene_id"></param>
        /// <returns></returns>
        private static string CreateTicketByNewAccessToken(string scene_id)
        {
            var token = AccessToken.NewInstance;

            if (string.IsNullOrEmpty(token.Access_Token))
                throw new ArgumentNullException("Access_Token");

            string url = string.Format(WeiXinConst.WeiXin_Ticket_CreateUrl, token.Access_Token);
            string postData = WeiXinConst.WeiXin_QrCodeTicket_Create_JsonString.Replace("{0}", scene_id);

            Ticket ticket = HttpClientHelper.PostResponse<Ticket>(url, postData);

            if (ticket == null || string.IsNullOrEmpty(ticket.ticket))
                throw new Exception("Ticket为Null，AccessToken:" + token.Access_Token);
            return ticket.ticket;
        }


        /// <summary>
        /// 根据Ticket获取二维码图片保存在本地
        /// </summary>
        /// <param name="scene_id">二维码场景id</param>
        /// <param name="imgPath">图片存储路径</param>
        public static void SaveQrCodeImage(string scene_id, string imgPath)
        {
            string Ticket = CreateTicket(scene_id);

            if (Ticket == null)
                throw new ArgumentNullException("Ticket");

            //ticket需 urlEncode
            string stUrl = string.Format(WeiXinConst.WeiXin_QrCode_GetUrl, HttpUtility.UrlEncode(Ticket));

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(stUrl);

            req.Method = "GET";

            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                string strpath = myResponse.ResponseUri.ToString();

                WebClient mywebclient = new WebClient();

                try
                {
                    mywebclient.DownloadFile(strpath, imgPath);
                }
                catch (Exception)
                {
                    throw new Exception("获取二维码图片失败！");
                }
            }
        }

        /// <summary>
        /// 根据SceneId 获取 二维码图片流
        /// </summary>
        /// <param name="scene_id"></param>
        /// <returns></returns>
        public static Stream GetQrCodeImageStream(string scene_id)
        {
            string Ticket = CreateTicket(scene_id);

            if (Ticket == null)
                throw new ArgumentNullException("Ticket");

            //ticket需 urlEncode
            string strUrl = string.Format(WeiXinConst.WeiXin_QrCode_GetUrl, HttpUtility.UrlEncode(Ticket));
            try
            {
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                Byte[] bytes = client.GetByteArrayAsync(strUrl).Result;
                return new MemoryStream(bytes);
            }
            catch
            {
                throw new Exception("获取二维码图片失败！");
            }
        }

        #endregion

        #region 根据经纬度 获取地址信息 BaiduApi

        /// <summary>
        /// 根据经纬度  获取 地址信息
        /// </summary>
        /// <param name="lat">经度</param>
        /// <param name="lng">纬度</param>
        /// <returns></returns>
        public static BaiDuGeoCoding GeoCoder(string lat, string lng)
        {
            string url = string.Format(WeiXinConst.Baidu_GeoCoding_ApiUrl, lat, lng);

            var model = HttpClientHelper.GetResponse<BaiDuGeoCoding>(url);

            return model;
        }

        #endregion

        #region 创建、查询、删除菜单

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="menuData">菜单字符串</param>
        /// <returns>ReturnMsg.ErrCode为0则创建菜单成功</returns>
        public static ErrorMessage CreateMenu(string menuData)
        {
            string url = string.Format(WeiXinConst.WeiXin_Menu_CreateUrl, AccessToken.Instance.Access_Token);
            ErrorMessage msg = HttpClientHelper.PostResponse<ErrorMessage>(url, menuData);
            return msg;
        }

        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <returns>菜单详细信息的字符串（失败则返回 null）</returns>
        public static string GetMenu()
        {
            string url = string.Format(WeiXinConst.WeiXin_Menu_GetUrl, AccessToken.Instance.Access_Token);
            string result = HttpClientHelper.GetResponse(url);
            return result;
        }

        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        /// <returns></returns>
        public static bool DeleteMenu()
        {
            string url = string.Format(WeiXinConst.WeiXin_Menu_DeleteUrl, AccessToken.Instance.Access_Token);
            ErrorMessage msg = HttpClientHelper.GetResponse<ErrorMessage>(url);
            return msg.ErrCode == "0";
        }

        #endregion

        #region 生成预支付账单 V2 App支付

        /// <summary>
        /// 生成预支付订单
        /// </summary>
        /// <param name="postData">请求参数</param>
        /// <returns></returns>
        public static PrePayMessage PreWXPay(string postData)
        {
            var accessToken = AccessToken.Instance;
            string url = string.Format(WeiXinConst.WeiXin_Pay_PrePayUrl, accessToken.Access_Token);
            PrePayMessage result = HttpClientHelper.PostResponse<PrePayMessage>(url, postData);

            if (result.TokenExpired)
                return PreWXPayByNewAccessToken(postData);

            return result;
        }

        private static PrePayMessage PreWXPayByNewAccessToken(string postData)
        {
            var accessToken = AccessToken.NewInstance;
            string url = string.Format(WeiXinConst.WeiXin_Pay_PrePayUrl, accessToken.Access_Token);
            PrePayMessage result = HttpClientHelper.PostResponse<PrePayMessage>(url, postData);
            return result;
        }

        #endregion

        #region V3 统一支付接口

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="isApp"></param>
        /// <returns></returns>
        public static UnifiedPrePayMessage UnifiedPrePay(string postData)
        {
            string url = WeiXinConst.WeiXin_Pay_UnifiedPrePayUrl;
            var message = HttpClientHelper.PostXmlResponse<UnifiedPrePayMessage>(url, postData);
            return message;
        }

        #endregion

        #region V2&V3 订单查询

        #region V2 订单查询

        public static OrderQueryMessage OrderQuery(string postData)
        {
            var accessToken = AccessToken.Instance;
            string url = string.Format(WeiXinConst.WeiXin_Pay_OrderQueryUrl, accessToken.Access_Token);
            OrderQueryMessage result = HttpClientHelper.PostResponse<OrderQueryMessage>(url, postData);
            if (result.TokenExpired)
            {
                return OrderQueryByNewAccessToken(postData);
            }
            return result;
        }

        private static OrderQueryMessage OrderQueryByNewAccessToken(string postData)
        {
            var accessToken = AccessToken.NewInstance;
            string url = string.Format(WeiXinConst.WeiXin_Pay_OrderQueryUrl, accessToken.Access_Token);
            OrderQueryMessage result = HttpClientHelper.PostResponse<OrderQueryMessage>(url, postData);
            return result;
        }

        #endregion

        #region V3 订单查询

        /// <summary>
        /// V3 统一订单查询接口
        /// </summary>
        /// <param name="postXml"></param>
        /// <returns></returns>
        public static UnifiedOrderQueryMessage UnifiedOrderQuery(string postXml)
        {
            string url = WeiXinConst.WeiXin_Pay_UnifiedOrderQueryUrl;
            UnifiedOrderQueryMessage message = HttpClientHelper.PostXmlResponse<UnifiedOrderQueryMessage>(url, postXml);
            return message;
        }

        #endregion
       

        #endregion

        #region V2 发货通知 

        /// <summary>
        /// 发货通知
        /// </summary>
        /// <param name="postData">请求数据</param>
        /// <param name="isApp">是否为App</param>
        public static void DeliverNotify(string postData, bool isApp)
        {
            var accessToken = isApp ? AccessToken.Instance : AccessToken.Instance;
            string url = string.Format(WeiXinConst.WeiXin_Pay_DeliverNotifyUrl, accessToken.Access_Token);
            ErrorMessage msg = HttpClientHelper.PostResponse<ErrorMessage>(url, postData);
            if (msg.TokenExpired)
            {
                DeliverNotifyByNewAccessToken(postData, isApp);
            }
        }

        private static void DeliverNotifyByNewAccessToken(string postData, bool isApp)
        {
            var accessToken = isApp ? AccessToken.NewInstance : AccessToken.NewInstance;
            string url = string.Format(WeiXinConst.WeiXin_Pay_DeliverNotifyUrl, accessToken.Access_Token);
            HttpClientHelper.PostResponse(url, postData);
        }

        #endregion

        #region V3 申请退款

        /// <summary>
        /// 申请退款（V3接口）
        /// </summary>
        /// <param name="postData">请求参数</param>
        /// <param name="certPath">证书路径</param>
        /// <param name="certPwd">证书密码</param>
        public static bool Refund(string postData, string certPath, string certPwd)
        {
            string url = WeiXinConst.WeiXin_Pay_UnifiedOrderRefundUrl;
            RefundMessage message = RefundHelper.PostXmlResponse<RefundMessage>(url, postData, certPath, certPwd);
            return message.Success;
        }

        #endregion
    }
}
