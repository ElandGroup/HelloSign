using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RestSharp;
using SignWeb.Models;

namespace SignWeb.Controllers
{
    public class HomeController : Controller
    {
        public static readonly string Key = ConfigurationManager.AppSettings["Key"];
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Sign()
        {
            return View();
        }

        public JsonResult PostUrl()
        {

            string url = "api/v1/checksign";
            string baseApiUrl = "http://localhost:172";
            var client = new RestClient(baseApiUrl);
            var request = new RestRequest(url, Method.POST);
            request.RequestFormat = DataFormat.Json;
            PayData payData = GetParam();
            request.AddBody(payData.GetValues());
            IRestResponse response = client.Execute(request);
            string content =response.Content;
            return Json(new { Data = content }, JsonRequestBehavior.AllowGet);
        }

        private PayData GetParam()
        {
            PayData payData = new PayData();
            payData.SetValue("name", "xiaomiao");
            payData.SetValue("password", "1234");
            payData.SetValue("sign", payData.MakeSign(Key));
            return payData;
        }



        public JsonResult PostUrl2()
        {

            string url = "api/v1/checksign";
            string baseApiUrl = "http://localhost:172";
            var client = new RestClient(baseApiUrl);
            var request = new RestRequest(url, Method.POST);
            request.RequestFormat = DataFormat.Json;
            PayData payData = GetParam2();
            request.AddBody(payData);
            IRestResponse response = client.Execute(request);
            string content = response.Content;
            return Json(new { Data = content }, JsonRequestBehavior.AllowGet);
        }

        private PayData GetParam2()
        {
            PayData payData = new PayData();
            payData.SetValue("name", "xiaomiao");
            payData.SetValue("password", "1234");
            payData.SetValue("sign", payData.MakeSign("1234444"));
            return payData;
        }

    }
}