using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXin.Lib.Core.Models.Message
{
    public class OrderInfo
    {
        /// <summary>
        /// 查询结果状态码 0 标识成功
        /// </summary>
        public int? Ret_Code { get; set; }

        /// <summary>
        /// 查询结果出错信息
        /// </summary>
        public string Ret_Msg { get; set; }

        /// <summary>
        /// 返回信息中的编码方式
        /// </summary>
        public string Input_Charset { get; set; }

        /// <summary>
        /// 订单状态，0 成功，其他失败
        /// </summary>
        public int? Trade_State { get; set; }

        /// <summary>
        /// 交易模式，1为 即时到帐，其他保留
        /// </summary>
        public string Trade_Mode { get; set; }

        /// <summary>
        /// 财付通 商户号
        /// </summary>
        public string Partner { get; set; }

        /// <summary>
        /// 银行类型
        /// </summary>
        public string Bank_Type { get; set; }

        /// <summary>
        /// 银行订单号
        /// </summary>
        public string Bank_BillNo { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public int? Total_Fee { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string Fee_Type { get; set; }

        /// <summary>
        /// 通知Id
        /// </summary>
        public string Notify_Id { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string Transaction_Id { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string Out_Trade_NO { get; set; }

        /// <summary>
        /// 是否分账，false为无分账，true有分账
        /// </summary>
        public string Is_Split { get; set; }

        /// <summary>
        /// 是否退款，false为无退款，true为退款
        /// </summary>
        public string Is_Refund { get; set; }

        /// <summary>
        /// 商户数据包
        /// </summary>
        public string Attach { get; set; }

        /// <summary>
        /// 支付完成时间
        /// </summary>
        public string Time_End { get; set; }

        /// <summary>
        /// 物流费用
        /// </summary>
        public int? Transport_Fee { get; set; }
        /// <summary>
        /// 物品费用
        /// </summary>
        public int? ProductFee { get; set; }
        /// <summary>
        /// 折扣价格
        /// </summary>
        public int? Discount { get; set; }

        /// <summary>
        /// 换算成人民币之后的总金额，单位为分，一般看Total_Fee即可
        /// </summary>
        public int? Rmb_Total_Fee { get; set; }

    }
}
