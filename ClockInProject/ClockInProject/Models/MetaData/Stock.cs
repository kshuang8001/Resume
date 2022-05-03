using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClockInProject.Models
{
    public class Stock
    {
        public string Code { get; set; }  
        public string Name { get; set; }
        public string PEratio { get; set; }
        public string DividendYield { get; set; }
        public string PBratio { get; set; } 
    }
}