using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Maxima.Net.SDK.Domain.Utils
{
    public class UtilsApi
    {
        private static MD5 md5 { get; set; } = MD5.Create();
        public static string GerarHashMD5(object item)
        {
            var jsonObject = JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(jsonObject));
            string hashObject = System.Convert.ToBase64String(md5.Hash);
            return hashObject;
        }
    }
}