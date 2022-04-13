using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CalendarService
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IService1"。
    [ServiceContract]
    public interface IService1
    {
         
        [OperationContract]
        [WebInvoke(Method ="GET", UriTemplate = "/Tutorial/{UserID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<OrdOrder> GetAllTutorial(string UserID);
        // TODO: 在此新增您的服務作業
    } 

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
}
