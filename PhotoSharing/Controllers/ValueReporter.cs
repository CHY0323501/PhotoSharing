using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//紀錄錯誤數據前using
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Routing;

namespace PhotoSharing.Controllers
{
    public class ValueReporter: ActionFilterAttribute    //繼承ActionFilterAttribute類別
    {
        SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PhotoSharing"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        public bool IsLog { get; set; }

        void executeSql(string sql) {
            Conn.Open();
            cmd.Connection = Conn;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            Conn.Close();
        }

        //要記得到app_start中的filterconfig中註冊


        void LogValues(RouteData routedata) //在此物件中可以取得曾對哪個controller&action提出request
        {
            cmd.Parameters.Clear();

            var controllerName = routedata.Values["controller"];
            var actionName = routedata.Values["action"];
            var parame = routedata.Values["id"] == null ? "N/A" : routedata.Values["id"];

            string sql = "insert into actionLog(controllerName, actionName, parame) values(@controllerName, @actionName, @parame)";
            cmd.Parameters.AddWithValue("@controllerName", controllerName);
            cmd.Parameters.AddWithValue("@actionName", actionName);
            cmd.Parameters.AddWithValue("@parame", parame);

            executeSql(sql);

        }


        void RequestsLog() //不需要傳參數
        {
            cmd.Parameters.Clear();

            var ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            var host = HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
            var browser = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

            var requestType = HttpContext.Current.Request.RequestType;
            var userHostAddress = HttpContext.Current.Request.UserHostAddress;
            var userHostName = HttpContext.Current.Request.UserHostName;
            var userMethod = HttpContext.Current.Request.HttpMethod;

            string sql = "insert into RequestLog([ip],host ,browser,requestType,userHostAddress,userHostName,httpMethod ) ";
             sql+= "values(@ip,@host ,@browser,@requestType,@userHostAddress,@userHostName,@httpMethod)";
            cmd.Parameters.AddWithValue("@ip", ip);
            cmd.Parameters.AddWithValue("@host", host);
            cmd.Parameters.AddWithValue("@browser", browser);
            cmd.Parameters.AddWithValue("@requestType", requestType);
            cmd.Parameters.AddWithValue("@userHostAddress", userHostAddress);
            cmd.Parameters.AddWithValue("@userHostName", userHostName);
            cmd.Parameters.AddWithValue("@httpMethod", userMethod);

            executeSql(sql);

        }

        //覆寫ActionFilterAttribute抽象類別中的方法
        //在"提出"要求時即紀錄資料(onactionexecuting)

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //base.OnActionExecuting(filterContext);
            if(IsLog)
                LogValues(filterContext.RouteData);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //base.OnResultExecuted(filterContext);
            if (IsLog)
                RequestsLog();
        }

    }
}