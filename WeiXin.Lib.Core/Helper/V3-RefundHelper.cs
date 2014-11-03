using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WeiXin.Lib.Core.Helper
{
    /// <summary>
    /// V3退款帮助类
    /// </summary>
    public class RefundHelper
    {
        /// <summary>
        /// 证书验证的 post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求Url</param>
        /// <param name="postData">post数据</param>
        /// <param name="certPath">证书路径</param>
        /// <param name="certPwd">证书密码</param>
        /// <returns></returns>
        public static T PostXmlResponse<T>(string url, string postData, string certPath, string certPwd)
            where T : class,new()
        {
            HttpWebRequest hp = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            hp.ClientCertificates.Add(new X509Certificate2(certPath, certPwd));

            var encoding = System.Text.Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);

            hp.Method = "POST";

            hp.ContentType = "application/x-www-form-urlencoded";

            hp.ContentLength = data.Length;

            using (Stream ws = hp.GetRequestStream())
            {
                // 发送数据
                ws.Write(data, 0, data.Length);
                ws.Close();

                using (HttpWebResponse wr = (HttpWebResponse)hp.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream(), encoding))
                    {
                        return HttpClientHelper.XmlDeserialize<T>(sr.ReadToEnd());
                    }
                }
            }
        }

        //验证服务器证书
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
