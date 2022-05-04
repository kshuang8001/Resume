using ClockInProject.Models;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ClockInProject.Service
{
    public class ExcelService
    {
        ClockIn_Entity db = new ClockIn_Entity();
        SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ClockInConnectionString"].ConnectionString);
        SqlDataAdapter adp = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand();
         
        

        private DataTable QuerySql(string sql)
        {
            cmd = new SqlCommand();
            cmd.Connection = Conn;
            cmd.CommandText = sql;
            adp.SelectCommand = cmd;

            DataSet ds = new DataSet();
            adp.Fill(ds);

            return ds.Tables[0];
        }

        private void ExcuteSql(string sql, List<object> LstObj)
        {
            cmd = new SqlCommand();
            cmd.Connection = Conn;
            Conn.Open();
            cmd.CommandText = sql;
            for (int i = 0; i < LstObj.Count(); i++) 
                cmd.Parameters.AddWithValue("@Parameter" + i, LstObj[i]);

            cmd.ExecuteNonQuery();
            Conn.Close();
            //return ds.Tables[0];
        }

        public List<SelectListItem> GetAll()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem(){Text = "匯入",Value = "Import"});
            items.Add(new SelectListItem(){Text = "匯出",Value = "Export"});
            return items;
        }

        public List<SelectListItem> GetTableName()
        {
            FieldInfo[] fieldInfos = db.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add((new SelectListItem() { Text = "請選擇", Value = "請選擇", Selected=true }));
            foreach (var item in fieldInfos)
            {
                items.Add((new SelectListItem() { Text = item.Name.Split('<', '>')[1], Value = item.Name.Split('<', '>')[1] })); 
            } 
            return items;
        }

        public DataTable RenderDataTableToExcel(string StrTableName, string FileName)
        {
            MemoryStream ms = null; 
            string StrSql = "select * from " + StrTableName;

            DataTable dataTable = QuerySql(StrSql); 

            ms = WriteToXls(dataTable) as MemoryStream;

            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            data = null;
            ms = null;
            fs = null;

            return dataTable;
        }

        private Stream WriteToXls(DataTable dataTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
            HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;

            

            // handling header.
            foreach (DataColumn column in dataTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // handling value.
            int rowIndex = 1;

            foreach (DataRow row in dataTable.Rows)
            {
                HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;

                foreach (DataColumn column in dataTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        public DataTable RenderExcelToDataTable(string StrTableName, string StrFileName)
        {
            FileStream FsExcel = new FileStream(StrFileName, FileMode.Open);
            DataTable DtExcel = ReadFromXls(FsExcel);
            DtExcel.TableName = StrTableName;
            return DtExcel;
        }

        private DataTable ReadFromXls(Stream ExcelFileStream)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            HSSFSheet sheet = workbook.GetSheetAt(0) as HSSFSheet;

            DataTable table = new DataTable();

            HSSFRow headerRow = sheet.GetRow(0) as HSSFRow;
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;

            for (int i = 0; i < sheet.LastRowNum; i++)
            {
                HSSFRow row = sheet.GetRow(i + 1) as HSSFRow;
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString().Trim();
                }

                table.Rows.Add(dataRow);
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;

            return table;
        }
        public void UpdateToDatabase(DataTable dataTable)
        {
            
            string StrSQL = "SELECT name FROM sys.columns WHERE " +
                            "(object_id = (SELECT T1.object_id FROM sys.tables AS T1 WHERE (T1.name = '" + dataTable.TableName + "')))" +
                            "AND(column_id IN(SELECT T2.column_id FROM sys.index_columns AS T2," +
                            "(SELECT T1.name, T1.object_id FROM sys.tables AS T1 WHERE(T1.name = '" + dataTable.TableName + "')) AS T3 " +
                            "WHERE(T2.object_id = T3.object_id))) ";

            DataTable DtPrimary = QuerySql(StrSQL);

            foreach (DataRow dr in dataTable.Rows)
            {

            
                // 取得索引欄位.
                StringBuilder sbIndex = new StringBuilder();
                foreach (DataRow drP in DtPrimary.Rows)
                    sbIndex.Append(drP["name"] + ", ");
                sbIndex.Remove(sbIndex.Length - 2, 2);

                // 取得要比對的索引值(內容)為何.
                StringBuilder sbWhere = new StringBuilder();
                foreach (DataRow drP in DtPrimary.Rows)
                    sbWhere.Append("(" + drP["name"].ToString() + " = '" + dr[drP["name"].ToString()] + "')" + " AND ");
                sbWhere.Remove(sbWhere.Length - 5, 5);

                // 取得 要處理的欄位有哪些(INSERT).
                StringBuilder sbInsertColumns = new StringBuilder();
                foreach (DataColumn dc in dataTable.Columns)
                    sbInsertColumns.Append(dc.ColumnName + ", ");
                sbInsertColumns.Remove(sbInsertColumns.Length - 2, 2);

                List<object> LstObj = new List<object>();

                // 取的當前列要處理的欄位值有哪些(INSERT).
                //StringBuilder sbInsertValues = new StringBuilder();
                //foreach (DataColumn dc in dataTable.Columns)
                //    sbInsertValues.Append("N'" + dr[dc.ColumnName].ToString() + "', ");
                //sbInsertValues.Remove(sbInsertValues.Length - 2, 2);

                StringBuilder sbInsertValues = new StringBuilder();
                foreach (DataColumn dc in dataTable.Columns)
                {
                    sbInsertValues.Append("@Parameter" + LstObj.Count + ", ");
                    LstObj.Add(dr[dc.ColumnName].ToString());
                }
                sbInsertValues.Remove(sbInsertValues.Length - 2, 2);

                // 取的當前列要處理的欄位與欄位值有哪些(UPDATE).
                //StringBuilder sbUpdateTable = new StringBuilder();
                //foreach (DataColumn dc in dataTable.Columns)
                //{
                //    if (!DtPrimary.Columns.Contains(dc.ColumnName))
                //    { 

                //        sbUpdateTable.Append(dc + " = N'" + dr[dc] + "'" + ", ");
                //    }
                //}
                //sbUpdateTable.Remove(sbUpdateTable.Length - 2, 2);

                StringBuilder sbUpdateTable = new StringBuilder();
                foreach (DataColumn dc in dataTable.Columns)
                {
                    if (!DtPrimary.Columns.Contains(dc.ColumnName))
                    {

                        sbUpdateTable.Append(dc + " = @Parameter" + LstObj.Count() + ", ");
                        LstObj.Add(dr[dc.ColumnName].ToString());
                    }
                }
                sbUpdateTable.Remove(sbUpdateTable.Length - 2, 2);

                StringBuilder sbSqlString = new StringBuilder();
                sbSqlString.Append("IF NOT EXISTS(" + "SELECT " + sbIndex.ToString() + " FROM " + dataTable.TableName + " WHERE " + sbWhere + ")" + Environment.NewLine);
                sbSqlString.Append("    INSERT INTO " + dataTable.TableName + "(" + sbInsertColumns + ") VALUES(" + sbInsertValues + "); " + Environment.NewLine);
                sbSqlString.Append("ELSE" + Environment.NewLine);
                sbSqlString.Append("    UPDATE " + dataTable.TableName + " SET " + sbUpdateTable + " WHERE " + sbWhere + "; " + Environment.NewLine);

                ExcuteSql(sbSqlString.ToString(), LstObj);
            }
        }

        
    }
}