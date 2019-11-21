using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhotoSharing
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //自訂路由(name不同即可)
            //constraints可以限制url輸入的內容，若有不想被看到的內容可在此限制
            routes.MapRoute(
               name: "PhotoRoute",
               url: "photos/{id}",
               defaults: new { controller = "Photos", action = "Display" },
               constraints: new { id=@"^[1-3]$"}
           );
            //必須啟動自訂路由
            routes.MapMvcAttributeRoutes();


            //預設路由要放在自訂路由之後
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
