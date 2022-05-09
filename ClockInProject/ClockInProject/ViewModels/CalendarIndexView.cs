using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClockInProject.Models;

namespace ClockInProject.ViewModels
{
    public class CalendarIndexView
    {
        public SysUserCalendar sysUserCalendar { get; set; }

        private int IntPage = 0;
        public int Page
        {
            get
            {
                if (IntPage == 0)
                    IntPage = 1;

                return IntPage;
            }
            set
            {
                IntPage = value;
            }
        }

        private int IntPageSize = 0;
        public int PageSize
        {
            get
            {
                if (IntPageSize == 0)
                    IntPageSize = 10;

                return IntPageSize;
            }
            set
            {
                IntPageSize = value;
            }
        }
    }
}