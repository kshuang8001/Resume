using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Web.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;

namespace WebService
{

    [ServiceContract(Namespace = "WebService")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TutorialService
    {
        public TutorialService()
        {

        }
        // 若要使用 HTTP GET，請加入 [WebGet] 屬性 (預設的 ResponseFormat 為 WebMessageFormat.Json)。
        // 若要建立可傳回 XML 的作業，
        //     請加入 [WebGet(ResponseFormat=WebMessageFormat.Xml)]，
        //     並在作業主體中包含下列這行程式:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        private static List<string> lst = new List<string>(new string[] { "Arrays", "Queues", "Stacks" });
        [DataContract]
        public class OrdOrder
        {
            [DataMember]
            public string UserID;
            [DataMember]
            public string CalDate;
            [DataMember]
            public string MsgTitle;
            [DataMember]
            public string Msg;

            public OrdOrder(string StrUserID, string StrCalDate, string StrMsgTitle, string StrMsg)
            {
                this.UserID = StrUserID;
                this.CalDate = StrCalDate;
                this.MsgTitle = StrMsgTitle;
                this.Msg = StrMsg;
            }
                
        }

        [OperationContract]
        public string DoWork()
        {
            // 在此新增您的作業實作
            return "ewq";
        }

        private string connectionString = WebConfigurationManager.ConnectionStrings["CBsql_ZZConnectionString"].ConnectionString;
        
        // 在此新增其他作業，並以 [OperationContract] 來標示它們
        [WebGet(UriTemplate ="/Tutorial", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        public List<OrdOrder> GetAllTutorial()
        {
            List<OrdOrder> ordOrders = new List<OrdOrder>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string StrSQL = "SELECT * FROM SysUserCalendar";
                SqlCommand cmd = new SqlCommand(StrSQL, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ordOrders.Add(new OrdOrder (reader["UserID"].ToString(), reader["CalDate"].ToString(), reader["MsgTitle"].ToString(), reader["Msg"].ToString()) );
                }

            }
            
            return ordOrders;
        }

        [WebGet(UriTemplate = "/Tutorial/{Tutorialid}")]
        public string GetTuorialByID(string Tutorialid)
        {
            int PID;
            return lst[Convert.ToInt16(Tutorialid)];
        }

        [WebInvoke(Method ="POST", RequestFormat = WebMessageFormat.Json, UriTemplate ="/Tutorial", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void AddTutorial(string Str)
        {
            lst.Add(Str);
        }
    }
}
