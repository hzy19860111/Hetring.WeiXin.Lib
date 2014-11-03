using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WeiXin.Lib.WCF.Model;

namespace WeiXin.Lib.WCF
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IWeiXinService
    {

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        AccessToken GetAccessToken();

        /// <summary>
        /// 创建新的AccessToken
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        AccessToken CreateAccessToken();

    }
}
