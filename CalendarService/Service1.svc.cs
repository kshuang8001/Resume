using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Configuration;

namespace CalendarAPI
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "Service1"。
    // 注意: 若要啟動 WCF 測試用戶端以便測試此服務，請在 [方案總管] 中選取 Service1.svc 或 Service1.svc.cs，然後開始偵錯。
    public class Service1 : IService1
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["CBsql_ZZConnectionString"].ConnectionString;
         
        // 在此新增其他作業，並以 [OperationContract] 來標示它們

        public List<OrdOrder> GetAllTutorial(string UserID)
        {
            List<OrdOrder> ordOrders = new List<OrdOrder>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string StrSQL = "SELECT * FROM SysUserCalendar WHERE UserID='" + UserID + "'";
                SqlCommand cmd = new SqlCommand(StrSQL, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ordOrders.Add(new OrdOrder(reader["UserID"].ToString(), reader["CalDate"].ToString(), reader["MsgTitle"].ToString(), reader["Msg"].ToString()));
                }

            }

            return ordOrders;
        }

    }
}
