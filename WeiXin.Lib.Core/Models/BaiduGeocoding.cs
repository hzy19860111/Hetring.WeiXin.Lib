using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXin.Lib.Core.Models
{
    public class BaiDuGeoCoding
    {
        public int Status { get; set; }
        public Result Result { get; set; }
    }

    public class Result
    {
        public Location Location { get; set; }

        public string Formatted_Address { get; set; }

        public string Business { get; set; }

        public AddressComponent AddressComponent { get; set; }

        public string CityCode { get; set; }
    }

    public class AddressComponent
    {
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市名
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区县名
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 街道名
        /// </summary>
        public string Street { get; set; }

        public string Street_number { get; set; }

    }

    public class Location
    {
        public string Lng { get; set; }
        public string Lat { get; set; }
    }
}