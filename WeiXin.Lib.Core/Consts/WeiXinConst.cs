using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXin.Lib.Core.Consts
{
    /// <summary>
    /// 微信 需要用到的Url、Json常量
    /// </summary>
    public class WeiXinConst
    {
        #region Value Const

        /// <summary>
        /// 微信开发者 AppId
        /// </summary>
        public const string AppId = "您的AppId";


        /// <summary>
        /// 微信开发者 Secret
        /// </summary>
        public const string Secret = "您的Secret";


        /// <summary>
        /// V2:支付请求中 用于加密的秘钥Key，可用于验证商户的唯一性，对应支付场景中的AppKey
        /// </summary>
        public static string PaySignKey = "V2.PaySignKey";


        /// <summary>
        /// V2:财付通签名key
        /// V3:商户支付密钥 Key。登录微信商户后台，进入栏目【账户设置】 【密码安全 】【API 安全】 【API 密钥】 ，进入设置 API 密钥。
        /// </summary>
        public const string PartnerKey = "PartnerKey";

        /// <summary>
        /// 商户号
        /// </summary>
        public const string PartnerId = "PartnerId";

        /// <summary>
        /// 百度地图Api  Ak
        /// </summary>
        public const string BaiduAk = "bGdpEtC2wbIaxMmhG7reDayA";

        /// <summary>
        /// 用于验证 请求 是否来自 微信
        /// </summary>
        public const string Token = "Token";

        /// <summary>
        /// 证书文件 路径
        /// </summary>
        public const string CertPath = @"E:\cert\apiclient_cert.pem";


        /// <summary>
        /// 证书文件密码（默认为商户号）
        /// </summary>
        public const string CertPwd = "111";

        #endregion

        #region Url Const

        #region AccessTokenUrl

        /// <summary>
        /// 公众号 获取Access_Token的Url(需Format  0.AppId 1.Secret)
        /// </summary>
        private const string AccessToken_Url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 公众号 获取Token的Url
        /// </summary>
        public static string WeiXin_AccessTokenUrl { get { return string.Format(AccessToken_Url, AppId, Secret); } }

        #endregion

        #region 获取用户信息Url

        /// <summary>
        /// 根据Code 获取用户OpenId Url
        /// </summary>
        private const string User_GetOpenIdUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

        /// <summary>
        /// 根据Code 获取用户OpenId的Url 需要Format 0.code
        /// </summary>
        public static string WeiXin_User_OpenIdUrl { get { return string.Format(User_GetOpenIdUrl, AppId, Secret, "{0}"); } }

        /// <summary>
        /// 根据OpenId 获取用户基本信息 Url（需要Format0.access_token 1.openid）
        /// </summary>
        public const string WeiXin_User_GetInfoUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";

        #endregion

        #region OAuth2授权Url

        /// <summary>
        /// OAuth2授权Url，需要Format0.AppId  1.Uri  2.state
        /// </summary>
        private const string OAuth2_Url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect";

        /// <summary>
        /// OAuth2授权Url，需要Format  0.Uri  1.state
        /// </summary>
        public static string WeiXin_User_OAuth2Url { get { return string.Format(OAuth2_Url, AppId, "{0}", "{1}"); } }

        #endregion

        #region QrCode Url

        /// <summary>
        /// 创建获取QrCode的Ticket Url  需要Format 0 access_token
        /// </summary>
        public const string WeiXin_Ticket_CreateUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";

        /// <summary>
        /// 获取二维码图片Url,需要Format 0.ticket
        /// </summary>
        public const string WeiXin_QrCode_GetUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";

        #endregion

        #region Baidu 逆地理编码Url

        /// <summary>
        /// 经纬度  逆地理编码 Url  需要Format 0.ak  1.经度  2.纬度
        /// </summary>
        private const string BaiduGeoCoding_ApiUrl = "http://api.map.baidu.com/geocoder/v2/?ak={0}&location={1},{2}&output=json&pois=0";

        /// <summary>
        /// 经纬度  逆地理编码 Url  需要Format 0.经度  1.纬度 
        /// </summary>
        public static string Baidu_GeoCoding_ApiUrl
        {
            get
            {
                return string.Format(BaiduGeoCoding_ApiUrl, BaiduAk, "{0}", "{1}");
            }
        }

        #endregion

        #region Menu Url

        /// <summary>
        /// 创建菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXin_Menu_CreateUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";

        /// <summary>
        /// 获取菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXin_Menu_GetUrl = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";

        /// <summary>
        /// 删除菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXin_Menu_DeleteUrl = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";


        #endregion

        #region 支付相关Url

        /// <summary>
        /// 生成预支付账单Url ，需替换 0 access_token
        /// </summary>
        public const string WeiXin_Pay_PrePayUrl = "https://api.weixin.qq.com/pay/genprepay?access_token={0}";

        /// <summary>
        /// 订单查询Url ，需替换0 access_token
        /// </summary>
        public const string WeiXin_Pay_OrderQueryUrl = "https://api.weixin.qq.com/pay/orderquery?access_token={0}";

        /// <summary>
        /// 发货通知Url，需替换 0 access_token
        /// </summary>
        public const string WeiXin_Pay_DeliverNotifyUrl = "https://api.weixin.qq.com/pay/delivernotify?access_token={0}";

        #region 统一支付相关Url （V3接口）

        /// <summary>
        /// 统一预支付Url
        /// </summary>
        public const string WeiXin_Pay_UnifiedPrePayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 订单查询Url 
        /// </summary>
        public const string WeiXin_Pay_UnifiedOrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";

        /// <summary>
        /// 退款申请Url
        /// </summary>
        public const string WeiXin_Pay_UnifiedOrderRefundUrl = "https://api.mch.weixin.qq.com/secapi/pay/refund";

        #endregion


        #endregion

        #endregion

        #region Json Const

        /// <summary>
        /// 获取二维码 所需Ticket 需要上传的Json字符串（需要Format 0.scene_id）
        /// </summary>
        /// <remarks>scene_id场景值ID  永久二维码时最大值为100000（目前参数只支持1--100000）</remarks>
        public const string WeiXin_QrCodeTicket_Create_JsonString = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\":{0}}}}";

        #endregion

    }
}
