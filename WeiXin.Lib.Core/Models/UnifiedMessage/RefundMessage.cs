using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiXin.Lib.Core.Models.UnifiedMessage
{
    /// <summary>
    /// 退款申请 消息
    /// </summary>
    [XmlRoot("xml")]
    public class RefundMessage : ReturnMessage
    {
        [XmlElement("result_code")]
        public string Result_Code { get; set; }
        [XmlElement("err_code")]
        public string Err_Code { get; set; }
        [XmlElement("err_code_des")]
        public string Err_Code_Des { get; set; }
        [XmlElement("appid")]
        public string AppId { get; set; }
        [XmlElement("mch_id")]
        public string Mch_Id { get; set; }
        [XmlElement("transaction_id")]
        public string Transaction_Id { get; set; }
        [XmlElement("out_trade_no")]
        public string Out_Trade_No { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        [XmlElement("out_refund_no")]
        public string Out_Refund_No { get; set; }

        /// <summary>
        /// 微信退款单号
        /// </summary>
        [XmlElement("refund_id")]
        public string Refund_Id { get; set; }

        /// <summary>
        /// 退款总金额,单位为分,可以做部分退款
        /// </summary>
        [XmlElement("refund_fee")]
        public string Refund_Fee { get; set; }

        /// <summary>
        /// 退款申请是否接收成功（不代表退款成功，退款结果需要查询）
        /// </summary>
        public bool Success
        {
            get
            {
                return Return_Code.ToLower() == "success"
                    && Result_Code.ToLower() == "success";
            }
        }
    }
}
