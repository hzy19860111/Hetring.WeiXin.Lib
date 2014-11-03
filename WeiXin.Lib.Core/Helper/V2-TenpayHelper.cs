using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WeiXin.Lib.Core.Helper.TenpayLib
{
    /// <summary>
    /// V2 接口 退款帮助类
    /// </summary>
    public class TenpayHelper
    {
        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="partnerId">商户号</param>
        /// <param name="partnerKey">财付通密钥</param>
        /// <param name="orderNo">商户订单号</param>
        /// <param name="transanctionId">微信订单号</param>
        /// <param name="totalFee">总金额（单位：分）</param>
        /// <param name="refundNo">退款单号</param>
        /// <param name="refundFee">退款金额（单位：分）</param>
        /// <param name="Context">MVC页面请求上下文对象</param>
        /// <returns></returns>
        public static bool Refund(string partnerId, string partnerKey, string orderNo,
            string transanctionId, string totalFee, string refundNo, string refundFee,
            string certPath, string certPwd)
        {
            //商户号
            string partner = partnerId;
            //创建请求对象
            RequestHandler reqHandler = new RequestHandler();
            //通信对象
            TenpayHttpClient httpClient = new TenpayHttpClient();
            //应答对象
            ClientResponseHandler resHandler = new ClientResponseHandler();

            //-----------------------------
            //设置请求参数
            //-----------------------------
            reqHandler.init();
            reqHandler.setKey(partnerKey); //财付通密钥

            reqHandler.setGateUrl("https://mch.tenpay.com/refundapi/gateway/refund.xml");

            reqHandler.setParameter("partner", partner);
            //out_trade_no和transaction_id至少一个必填，同时存在时transaction_id优先
            //
            if (string.IsNullOrEmpty(transanctionId))
            {
                if (string.IsNullOrEmpty(orderNo))
                    throw new Exception("订单号不能为空！");

                reqHandler.setParameter("out_trade_no", orderNo);
            }
            else
            {
                reqHandler.setParameter("transaction_id", transanctionId);
            }

            reqHandler.setParameter("out_refund_no", refundNo); //退款单号
            reqHandler.setParameter("total_fee", totalFee); //总金额
            reqHandler.setParameter("refund_fee", refundFee); //退款金额
            reqHandler.setParameter("op_user_id", partnerId); //todo:配置（op_user_id,op_user_passwd)
            reqHandler.setParameter("op_user_passwd", MD5Util.GetMD5("111111", "GBK"));
            reqHandler.setParameter("service_version", "1.1");

            string requestUrl = reqHandler.getRequestURL();
            httpClient.setCertInfo(certPath, certPwd);
            //设置请求内容
            httpClient.setReqContent(requestUrl);
            //设置超时
            httpClient.setTimeOut(10);

            string rescontent = "";
            //后台调用
            if (httpClient.call())
            {
                //获取结果
                rescontent = httpClient.getResContent();

                resHandler.setKey(partnerKey);
                //设置结果参数
                resHandler.setContent(rescontent);
                //判断签名及结果
                if (resHandler.isTenpaySign() && resHandler.getParameter("retcode") == "0")
                {
                    ////商户订单号
                    //string out_trade_no = resHandler.getParameter("out_trade_no");
                    ////财付通订单号
                    //string transaction_id = resHandler.getParameter("transaction_id");
                    ////业务处理
                    //  Response.Write("OK,transaction_id=" + resHandler.getParameter("transaction_id") + "<br>");
                    return true;
                }
                else
                {
                    return false;
                    //错误时，返回结果未签名。
                    //如包格式错误或未确认结果的，请使用原来订单号重新发起，确认结果，避免多次操作
                    //  Response.Write("业务错误信息或签名错误:" + resHandler.getParameter("retcode") + "," + resHandler.getParameter("retmsg") + "<br>");
                }
            }
            else
            {
                return false;
                //后台调用通信失败
                // Response.Write("call err:" + httpClient.getErrInfo() + "<br>" + httpClient.getResponseCode() + "<br>");
                //有可能因为网络原因，请求已经处理，但未收到应答。
            }
            //获取debug信息,建议把请求、应答内容、debug信息，通信返回码写入日志，方便定位问题
            //  Response.Write("http res:" + httpClient.getResponseCode() + "," + httpClient.getErrInfo() + "<br>");
            //   Response.Write("req url:" + requestUrl + "<br/>");
            //  Response.Write("req debug:" + reqHandler.getDebugInfo() + "<br/>");
            //  Response.Write("res content:" + Server.HtmlEncode(rescontent) + "<br/>");
            //  Response.Write("res debug:" + Server.HtmlEncode(resHandler.getDebugInfo()) + "<br/>");
        }
    }
}
