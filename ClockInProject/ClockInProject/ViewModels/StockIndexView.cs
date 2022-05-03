using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClockInProject.ViewModels
{
    public class StockIndexView
    {
        [DisplayName("股票代號")]
        [Required(ErrorMessage = "請輸入股票代號")]
        public string Code { get; set; }

        public string Name { get; set; }
        public string PEratio { get; set; }
        public string DividendYield { get; set; }
        public string PBratio { get; set; }
    }
}