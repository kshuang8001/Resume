using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using PagedList.Mvc;
using PagedList;

namespace ClockInProject.ViewModels
{
    public class ExcelIndexView
    {
        //[Required]
        //[FileExtensions(Extensions = ".xls", ErrorMessage = "Incorrect file format")]
        //public HttpPostedFileBase fileupload { get; set; }

        public DataTable dataTable { get; set; }

        public string DrpTableName { get; set; }
         
        [Required(ErrorMessage ="請選擇匯出/匯入")]
        public string RdnList1 { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }



    }
}