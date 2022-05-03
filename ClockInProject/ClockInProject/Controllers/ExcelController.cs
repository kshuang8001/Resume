using ClockInProject.Models;
using ClockInProject.Service;
using ClockInProject.ViewModels;
using System.IO;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace ClockInProject.Controllers
{
    public class ExcelController : Controller
    {
        ExcelService excelService = new ExcelService();
        ClockIn_Entity db = new ClockIn_Entity();
        // GET: Excel 
        public ActionResult Index()
        { 
            ViewBag.TableItems = excelService.GetTableName();
             
            return View();
        }

        [HttpPost]
        public ActionResult Index(ExcelIndexView excelIndexView)
        {
            if (ModelState.IsValid)
            {
                if (excelIndexView.RdnList1 == "Export")
                {
                    //匯出
                     
                    excelIndexView.dataTable = excelService.RenderDataTableToExcel(excelIndexView.DrpTableName, "D:\\" + excelIndexView.DrpTableName + ".xls");
                    
                }
                else
                {

                    //string path = Path.Combine(Server.MapPath($"~/Images"), excelIndexView.fileupload.FileName);
                    //匯入
                    excelIndexView.dataTable = excelService.RenderExcelToDataTable(excelIndexView.DrpTableName, @"D:\" + excelIndexView.DrpTableName + @".xls");
                    excelService.UpdateToDatabase(excelIndexView.dataTable);
                }
            }

            ViewBag.TableItems = excelService.GetTableName();

            return View(excelIndexView);
        }
    }
}