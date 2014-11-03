using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXin.Lib.Core.Models
{
    /// <summary>  
    ///Ticket类   
    /// </summary>  
    public class Ticket
    {

        private string _ticket;
        private string _expire_seconds;

        /// <summary>
        /// 凭借此ticket可以在有效时间内换取二维码。  
        /// </summary>
        public string ticket
        {
            get { return _ticket; }
            set { _ticket = value; }
        }

        /// <summary>  
        /// 凭证有效时间，单位：秒  
        /// </summary>  
        public string expire_seconds
        {
            get { return _expire_seconds; }
            set { _expire_seconds = value; }
        }
    }
}
