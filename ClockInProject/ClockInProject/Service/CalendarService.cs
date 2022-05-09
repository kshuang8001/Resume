using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClockInProject.Service
{
    public class CalendarService
    {
        public bool Between(string str, string str2, string str3)
        {
            if(Convert.ToInt32(str) >= Convert.ToInt32(str2) && Convert.ToInt32(str) <= Convert.ToInt32(str3))
            {
                return true;
            }
             
            return false;
        }
    }
}