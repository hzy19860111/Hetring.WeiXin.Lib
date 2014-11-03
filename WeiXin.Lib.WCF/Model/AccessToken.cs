using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiXin.Lib.Core.Helper;

namespace WeiXin.Lib.WCF.Model
{
    /// <summary>
    /// AccessToken类，公众号通过此token 获取相关信息 （单例类）
    /// </summary>
    public sealed class AccessToken
    {
        private static AccessToken _Token = new AccessToken();

        private static object lock_Object = new object();

        /// <summary>
        /// 此处 会判断是否过期，没过期返回原存储的Token
        /// </summary>
        public static AccessToken Instance
        {
            get
            {
                if (_Token.Expired)
                {
                    lock (lock_Object)
                    {
                        if (_Token.Expired)
                        {
                            _Token.CreateTime = DateTime.Now;
                            _Token.CopyModel(JsonConvert.DeserializeObject<AccessToken>(WeiXinHelper.GetAccessToken()));
                        }
                    }
                }

                return _Token;
            }
        }

        /// <summary>
        /// 此处会创建新的Token返回，只有在调用接口提示AccessToken过期时 才调用这个接口。
        /// </summary>
        /// <returns></returns>
        public static AccessToken CreateNewInstance()
        {
            lock (lock_Object)
            {
                _Token.CreateTime = DateTime.Now;
                _Token.CopyModel(JsonConvert.DeserializeObject<AccessToken>(WeiXinHelper.GetAccessToken()));
            }
            return _Token;
        }

        private AccessToken()
        {
            CreateTime = DateTime.Now;
            _expiresIn = -1;
        }

        private string _accessToken;
        private int _expiresIn;
        public DateTime CreateTime;
        public string Access_Token
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        /// <summary>
        /// 有效时间，秒
        /// </summary>
        public int Expires_In
        {
            get { return _expiresIn; }
            set { _expiresIn = value; }
        }

        public bool Expired
        {
            get
            {
                DateTime expiredTime = CreateTime.AddSeconds(_expiresIn);

                if (DateTime.Now > expiredTime)
                    return true;

                return false;
            }
        }

        public void CopyModel(AccessToken token)
        {
            //token 为空，将 过期时间 设置为 -1
            if (token == null)
            {
                this.Expires_In = -1;
                return;
            }

            this.Access_Token = token.Access_Token;
            this.Expires_In = token.Expires_In;

        }
    }
}