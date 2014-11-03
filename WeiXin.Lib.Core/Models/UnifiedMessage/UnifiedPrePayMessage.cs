using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiXin.Lib.Core.Models.UnifiedMessage
{
    /// <summary>
    /// V3 统一支付消息
    /// </summary>
    [XmlRoot("xml")]
    public class UnifiedPrePayMessage : ReturnMessage
    {
        [XmlElement("result_code")]
        public string Result_Code { get; set; }
        [XmlElement("appid")]
        public string AppId { get; set; }
        [XmlElement("mch_id")]
        public string Mch_Id { get; set; }
        [XmlElement("nonce_str")]
        public string Nonce_Str { get; set; }
        [XmlElement("sign")]
        public string Sign { get; set; }
        [XmlElement("err_code")]
        public string Err_Code { get; set; }
        [XmlElement("err_code_des")]
        public string Err_Code_Des { get; set; }

        [XmlElement("trade_type")]
        public string Trade_Type { get; set; }
        [XmlElement("prepay_id")]
        public string Prepay_Id { get; set; }

        [XmlElement("code_url")]
        /// <summary>
        /// Trade_Type 为Native时 返回
        /// </summary>
        public string Code_Url { get; set; }

        [XmlIgnore]
        public bool ResultSuccess
        {
            get { return this.Result_Code.ToLower() == "success"; }
        }

        [XmlIgnore]
        public bool ReturnSuccess
        {
            get { return Return_Code.ToLower() == "success"; }
        }
    }
}
