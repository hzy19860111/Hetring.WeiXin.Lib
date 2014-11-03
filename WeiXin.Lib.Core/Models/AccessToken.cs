using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXin.Lib.Core.Models
{
    /// <summary>
    /// AccessToken类
    /// </summary>
    public sealed class AccessToken
    {

        private static readonly WeiXinService.WeiXinServiceClient client = new WeiXinService.WeiXinServiceClient();

        /// <summary>
        /// 此处获取AccessToken
        /// </summary>
        public static WeiXinService.AccessToken Instance
        {
            get
            {
                return client.GetAccessToken();
            }
        }

        /// <summary>
        /// 此处 会 重新到微信获取AccessToken
        /// </summary>
        public static WeiXinService.AccessToken NewInstance
        {
            get
            {
                return client.CreateAccessToken();
            }
        }
    }
}
