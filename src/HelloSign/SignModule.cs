using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.IO;
using Nancy.ModelBinding;
using Newtonsoft.Json;

namespace HelloSign
{
    public class SignModule:NancyModule
    {
        public static readonly string key = "pangpang";
        public SignModule():base("api/v1")
        {
            Post["/checksign"] = _ =>
              {

                  RequestStream requestStream = this.Request.Body;
                  byte[] bytes = new byte[requestStream.Length];
                  requestStream.Read(bytes, 0, (int)requestStream.Length);
                  string jsonString = Encoding.UTF8.GetString(bytes);

                  PayData payData = new PayData();
                  payData.FromJson(jsonString);

                  return payData.CheckSign(key);
              };
        }

    }
}
