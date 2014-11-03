using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiXin.Lib.Core.Models.UnifiedMessage
{
    /// <summary>
    /// 支付通知 消息
    /// </summary>
    [XmlRoot("xml")]
    public class NotifyMessage : ReturnMessage
    {
        #region 以下在Return_Code 为Success时返回
        [XmlElement("appid")]
        public string AppId { get; set; }
        [XmlElement("mch_id")]
        public string Mch_Id { get; set; }
        [XmlElement("result_code")]
        public string Result_Code { get; set; }
        [XmlElement("err_code")]
        public string Err_Code { get; set; }
        [XmlElement("err_code_des")]
        public string Err_Code_Des { get; set; }
        #endregion

        #region 以下在Return_Code和Result_Code都为Success时返回
        [XmlElement("openid")]
        public string OpenId { get; set; }
        [XmlElement("is_subscribe")]
        public string Is_Subscribe { get; set; }
        [XmlElement("trade_type")]
        public string Trade_Type { get; set; }
        [XmlElement("bank_type")]
        public string Bank_Type { get; set; }
        [XmlElement("total_fee")]
        public int Total_Fee { get; set; }
        [XmlElement("transaction_id")]
        public string Transaction_Id { get; set; }
        [XmlElement("out_trade_no")]
        public string Out_Trade_No { get; set; }
        [XmlElement("time_end")]
        public string Time_End { get; set; }
        #endregion

        [XmlIgnore]
        public bool ResultSuccess
        {
            get
            {
                return
                    this.Return_Code.ToLower() == "success"
                    && this.Result_Code.ToLower() == "success";
            }
        }
    }
}
