using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeiXin.Lib.Core.Models.UnifiedMessage
{
    /// <summary>
    /// 消息基类
    /// </summary>
    public class ReturnMessage
    {
        [XmlElement("return_code")]
        public string Return_Code { get; set; }

        [XmlElement("return_msg")]
        public string Return_Msg { get; set; }

        public string ToXmlString()
        {
            return string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code>
                    <return_msg><![CDATA[{1}]]></return_msg></xml>", Return_Code, Return_Msg);
        }
    }
}
