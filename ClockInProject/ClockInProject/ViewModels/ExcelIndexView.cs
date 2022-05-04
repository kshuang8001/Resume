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

        private int IntPage = 0;
        public int Page {
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
        public int PageSize {
            get {
                if (IntPageSize == 0)
                    IntPageSize = 10;

                return IntPageSize;
            }
            set {
                IntPageSize = value;
            }
        }

        public List<DataRow> LstDr { get; set; }//Regular list to hold data from Datatable
        public PagedList<DataRow> PagLstDr { get; set; }

    }
}