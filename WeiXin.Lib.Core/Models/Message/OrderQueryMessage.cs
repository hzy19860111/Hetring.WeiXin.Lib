using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXin.Lib.Core.Models.Message
{
    public class OrderQueryMessage : ErrorMessage
    {
        public OrderInfo Order_Info { get; set; }

        /// <summary>
        /// 判断订单 支付结果
        /// </summary>
        public bool Success
        {
            get
            {
                return Order_Info != null
                    && Order_Info.Ret_Code.HasValue && Order_Info.Ret_Code.Value == 0
                    && Order_Info.Trade_State.HasValue && Order_Info.Trade_State.Value == 0;
            }
        }
    }
}
