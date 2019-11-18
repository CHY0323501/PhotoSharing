using PhotoSharing.Controllers;
using System.Web;
using System.Web.Mvc;

namespace PhotoSharing
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //以下為處理錯誤狀況的filter
            filters.Add(new HandleErrorAttribute());

            //須將剛剛建立的紀錄檔類別註冊在此(在此註冊是應用程式層級，全部應用程式都會記錄或到controller紀錄controller等級、action等級)
            filters.Add(new ValueReporter() { IsLog=true});
        }
    }
}
