using System.Web.Mvc;
 

namespace ClockInProject.Controllers
{
     

    [RequireHttps]
    [Authorize]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
             

            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }
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
         
        //public ActionResult Logout()
        //{
        //    //清空Cookie
        //    int IntCookies = Request.Cookies.Count;

        //    for (int i = 0; i < IntCookies; i++)
        //    {
        //        HttpCookie cookie = Request.Cookies[i];
        //        cookie.Expires = DateTime.Now.AddDays(-1);
        //        cookie.Values.Clear();
        //        cookie.Value = string.Empty;
        //        Response.Cookies.Set(cookie); 
        //    }
        //    //清空驗證
        //    FormsAuthentication.SignOut();
        //    //傳到前端執行FB登出
        //    TempData["Logout"] = true;
            
        //    return RedirectToAction("Index", "Login");
        //}
    }
}