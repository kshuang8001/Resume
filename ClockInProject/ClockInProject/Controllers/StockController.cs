using ClockInProject.Models;
using ClockInProject.Service;
using ClockInProject.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClockInProject.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock
        private string StrBaseurl = "https://openapi.twse.com.tw/v1/exchangeReport/BWIBBU_ALL";
        StockService stockService = new StockService();

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string Code)
        {
            //SSL/TLS通道開啟
            ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            StockIndexView stockIndexView = new StockIndexView();

            using (var client = new HttpClient())
            {
                //初始化
                //client.BaseAddress = new Uri(StrBaseurl);
                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync(StrBaseurl);

                //讀取成功  
                if (Res.IsSuccessStatusCode)
                {
                    //API結果篩進LIST   
                    var StockResponse = Res.Content.ReadAsStringAsync().Result;
                    stockIndexView = JsonConvert.DeserializeObject<List<StockIndexView>>(StockResponse).Where(x => x.Code == Code).FirstOrDefault();

                }

            }



            //Dictionary<int, string> categorys = stockService.GetAll();
            //List<SelectListItem> LstItems = new List<SelectListItem>();
            //foreach (KeyValuePair<int, string> item in categorys)
            //{
            //    LstItems.Add(new SelectListItem()
            //    {
            //        Text = item.Value,
            //        Value = item.Key.ToString()
            //    });
            //}

            //ViewBag.categorys = LstItems;
            return PartialView("PartialView/_PartialIndex", stockIndexView);
        }


    }
}