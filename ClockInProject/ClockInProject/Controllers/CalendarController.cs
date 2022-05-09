using ClockInProject.Models;
using ClockInProject.Service;
using ClockInProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace ClockInProject.Controllers
{  
    public class CalendarController : Controller
    {
        CalendarService calendarService = new CalendarService();
        ClockIn_Entity db = new ClockIn_Entity();
        const int page = 1;
        const int pageSize = 10;
        // GET: Calendar
        public ActionResult Index(CalendarIndexView calendarIndexView)
        {
            IEnumerable<SysUserCalendar> sysUserCalendars =
                db.SysUserCalendar.Where(x => x.UserID == User.Identity.Name).OrderBy(x=> x.AutoNo).ToPagedList(calendarIndexView.Page, calendarIndexView.PageSize);

            foreach (SysUserCalendar sysUserCalendar in sysUserCalendars)
            {
                sysUserCalendar.CalDate = sysUserCalendar.CalDate.Substring(0, 4) + "/" + sysUserCalendar.CalDate.Substring(4, 2) + "/" + sysUserCalendar.CalDate.Substring(6, 2);
            }

            return View(sysUserCalendars);
        }

        [HttpPost]
        public ActionResult Index(string CalDate_Start, string CalDate_End)
        {
            IEnumerable<SysUserCalendar> sysUserCalendars =
                db.SysUserCalendar.Where(x => x.UserID == User.Identity.Name && 
                (x.CalDate.CompareTo(CalDate_Start.Replace("-", "")) >= 0 &&
                x.CalDate.CompareTo(CalDate_End.Replace("-", "")) < 0)
                ).OrderBy(x => x.AutoNo).ToPagedList(page, pageSize);

            foreach (SysUserCalendar sysUserCalendar in sysUserCalendars) 
                sysUserCalendar.CalDate = sysUserCalendar.CalDate.Substring(0, 4) + "/" + sysUserCalendar.CalDate.Substring(4, 2) + "/" + sysUserCalendar.CalDate.Substring(6, 2);
            

            return View(sysUserCalendars);
        }

        [HttpPost]
        public ActionResult New(CalendarIndexView calendarIndexView)
        {

            return View();
        }

        public ActionResult New()
        {
            return View();
        }
    }
}