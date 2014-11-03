using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiXin.Lib.Core.Models.UnifiedMessage
{
    /// <summary>
    /// V3 订单统一查询消息
    /// </summary>
    [XmlRoot("xml")]
    public class UnifiedOrderQueryMessage : ReturnMessage
    {
        #region 以下字段在 Return_Code为 Success时 返回

        /// <summary>
        /// 公众账号ID
        /// </summary>
        [XmlElement("appid")]
        public string AppId { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        [XmlElement("mch_id")]
        public string Mch_Id { get; set; }

        /// <summary>
        /// 业务结果（SUCCESS/FAIL）
        /// </summary>
        [XmlElement("result_code")]
        public string Result_Code { get; set; }

        /// <summary>
        /// 错误代码（ORDERNOTEXIST订单不存在、SYSTEMERROR系统错误）
        /// </summary>
        [XmlElement("err_code")]
        public string Err_Code { get; set; }

        /// <summary>
        /// 错误代码描述
        /// </summary>
        [XmlElement("err_code_des")]
        public string Err_Code_Des { get; set; }

        #endregion

        #region 以下字段在Return_Code和Result_Code 都为Success时返回

        /// <summary>
        /// 交易状态
        /// </summary>
        /// <remarks>
        /// SUCCESS—支付成功
        /// REFUND—转入退款
        /// NOTPAY—未支付
        /// CLOSED—已关闭
        /// REVOKED—已撤销
        /// USERPAYING--用户支付中
        /// NOPAY--未支付(输入密码或
        /// 确认支付超时)
        /// PAYERROR--支付失败(其他
        /// 原因，如银行返回失败)
        /// </remarks>
        [XmlElement("trade_state")]
        public string Trade_State { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        [XmlElement("openid")]
        public string OpenId { get; set; }

        /// <summary>
        /// 交易类型：JSAPI、NATIVE、MICROPAY 、APP
        /// </summary>
        [XmlElement("trade_type")]
        public string Trade_Type { get; set; }

        /// <summary>
        /// 付款银行 银行类型，采用字符串类型的银行标识
        /// </summary>
        [XmlElement("bank_type")]
        public string Bank_Type { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        [XmlElement("total_fee")]
        public string Total_Fee { get; set; }

        /// <summary>
        /// 交易完成时间
        /// </summary>
        [XmlElement("time_end")]
        public string Tiem_End { get; set; }

        #endregion

        /// <summary>
        /// 支付是否成功
        /// </summary>
        public bool Success
        {
            get
            {
                return
                    Return_Code.ToLower() == "success"
                    && this.Result_Code.ToLower() == "success"
                    && this.Trade_State.ToLower() == "success";
            }
        }


        public bool NotPrePay
        {
            get
            {
                return Return_Code.ToLower() == "success"
                    && !string.IsNullOrEmpty(Err_Code)
                && Err_Code.ToUpper() == "ORDERNOTEXIST";
            }
        }

    }
}
