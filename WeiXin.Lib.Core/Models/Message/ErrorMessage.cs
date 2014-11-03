using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXin.Lib.Core.Models.Message
{
    public class ErrorMessage
    {
        //{"errcode":40001,"errmsg":"invalid credential"} AppId AppSecret   配置错误，或AccessToken 过期

        public string ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public bool TokenExpired
        {
            get { return ErrCode == "40001"; }
        }
    }
}
