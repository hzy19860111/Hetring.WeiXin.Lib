using Mvc_Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WeiXin.Lib.Core.Consts;
using WeiXin.Lib.Core.Helper;
using WeiXin.Lib.Core.Helper.WXPay;
using WeiXin.Lib.Core.Models.UnifiedMessage;

namespace Mvc_Demo.Controllers
{
    public class V3JsapiController : Controller
    {
        //
        // GET: /V3Jsapi/


        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Pay()
        {
            string code = "";//网页授权获得的code
            string orderNo = ""; //文档中的out_trade_no
            string description = ""; //商品描述
            string totalFee = "1";//订单金额（单位：分）
            string createIp = "127.0.0.1";
            string notifyUrl = ""; //通知url
            string openId = WeiXinHelper.GetUserOpenId(code);//通过网页授权code获取用户openid（或者之前有存储用户的openid 也可以直接拿来用）

            //prepayid 只有第一次支付时生成，如果需要再次支付，必须拿之前生成的prepayid。
            //也就是说一个orderNo 只能对应一个prepayid
            string prepayid = string.Empty;

            #region 之前生成过 prepayid，此处可略过

            //创建Model
            UnifiedWxPayModel model = UnifiedWxPayModel.CreateUnifiedModel(WeiXinConst.AppId, WeiXinConst.PartnerId, WeiXinConst.PartnerKey);

            //预支付
            UnifiedPrePayMessage result = WeiXinHelper.UnifiedPrePay(model.CreatePrePayPackage(description, orderNo, totalFee, createIp, notifyUrl, openId));

            if (result == null
                    || !result.ReturnSuccess
                    || !result.ResultSuccess
                    || string.IsNullOrEmpty(result.Prepay_Id))
            {
                throw new Exception("获取PrepayId 失败");
            }

            //预支付订单
            prepayid = result.Prepay_Id;

            #endregion
            
            //JSAPI 支付参数的Model
            PayModel payModel = new PayModel()
            {
                AppId = model.AppId,
                Package = string.Format("prepay_id={0}", prepayid),
                Timestamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString(),
                Noncestr = CommonUtil.CreateNoncestr(),
            };

            Dictionary<string, string> nativeObj = new Dictionary<string, string>();
            nativeObj.Add("appId", payModel.AppId);
            nativeObj.Add("package", payModel.Package);
            nativeObj.Add("timeStamp", payModel.Timestamp);
            nativeObj.Add("nonceStr", payModel.Noncestr);
            nativeObj.Add("signType", payModel.SignType);
            payModel.PaySign = model.GetCftPackage(nativeObj); //生成JSAPI 支付签名


            return View(payModel);
        }

        /// <summary>
        /// 到微信服务器查询 订单支付的结果 (jsapi支付返回ok，我们要判断下服务器支付状态，如果没有支付成功，到微信服务器查询）
        /// </summary>
        /// <param name="orderNo"></param>
        public bool QueryOrder(string orderNo)
        {
            //这里应先判断服务器 订单支付状态，如果接到通知，并已经支付成功，就不用 执行下面的查询了
            UnifiedWxPayModel model = UnifiedWxPayModel.CreateUnifiedModel(WeiXinConst.AppId, WeiXinConst.PartnerId, WeiXinConst.PartnerKey);
            UnifiedOrderQueryMessage message = WeiXinHelper.UnifiedOrderQuery(model.CreateOrderQueryXml(orderNo));
            //此处主动查询的结果，只做查询用（不能作为支付成功的依据）
            return message.Success;
        }

        

        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="transaction_Id">微信交易单号</param>
        /// <param name="orderNo">我们自己的单号</param>
        /// <param name="totalFee">订单金额（分）</param>
        /// <param name="refundNo">退款单号（我们自己定义）</param>
        /// <param name="refundFee">退款金额（分）</param>
        /// <returns></returns>
        public bool UnifiedOrderRefund(string transaction_Id,string orderNo,string totalFee, string refundNo,string refundFee)
        {
            UnifiedWxPayModel model = UnifiedWxPayModel.CreateUnifiedModel(WeiXinConst.AppId, WeiXinConst.PartnerId, WeiXinConst.PartnerKey);
            string postData = model.CreateOrderRefundXml(orderNo, transaction_Id, totalFee, refundNo, refundFee);
            //退款需要用到证书， 要配置WeiXineConst CertPath 和 CertPwd
            return WeiXinHelper.Refund(postData, WeiXinConst.CertPath, WeiXinConst.CertPwd);
        }
        /// <summary>
        /// 微信支付通知（貌似比较臃肿，待优化）
        /// </summary>
        /// <returns></returns>
        public void Notify()
        {
            ReturnMessage returnMsg = new ReturnMessage() { Return_Code = "SUCCESS", Return_Msg = "" };
            string xmlString = GetXmlString(Request);
            NotifyMessage message = null;
            try
            {
                //此处应记录日志
                message = HttpClientHelper.XmlDeserialize<NotifyMessage>(xmlString);

                #region 验证签名并处理通知
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                string sign = string.Empty;
                foreach (XmlNode node in doc.FirstChild.ChildNodes)
                {
                    if (node.Name.ToLower() != "sign")
                        dic.Add(node.Name, node.InnerText);
                    else
                        sign = node.InnerText;
                }

                UnifiedWxPayModel model = UnifiedWxPayModel.CreateUnifiedModel(WeiXinConst.AppId, WeiXinConst.PartnerId, WeiXinConst.PartnerKey);
                if (model.ValidateMD5Signature(dic, sign))
                {
                    //处理通知
                }
                else
                {
                    throw new Exception("签名未通过！");
                }

                #endregion

            }
            catch (Exception ex)
            {
                //此处记录异常日志
                returnMsg.Return_Code = "FAIL";
                returnMsg.Return_Msg = ex.Message;
            }
            Response.Write(returnMsg.ToXmlString());
            Response.End();
        }

        /// <summary>
        /// 获取Post Xml数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string GetXmlString(HttpRequestBase request)
        {
            using (System.IO.Stream stream = request.InputStream)
            {
                Byte[] postBytes = new Byte[stream.Length];
                stream.Read(postBytes, 0, (Int32)stream.Length);
                return System.Text.Encoding.UTF8.GetString(postBytes);
            }
        }

    }
}
