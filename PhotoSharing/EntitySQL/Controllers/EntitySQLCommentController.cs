using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoSharing.EntitySQL.Models;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Collections;
using System.Net;

namespace PhotoSharing.EntitySQL.Controllers
{
    public class EntitySQLCommentController : Controller
    {
        //EntitySQL鑄造的物件跟ado.net很像
        EntityConnection Conn = new EntityConnection();
        public ActionResult Index()
        {
            Conn.ConnectionString = "Name=PhotoSharingEntities";        //需要把webconfig的屬性+連線名稱都寫出來，N要大寫
            var Cmd = Conn.CreateCommand();
            //寫sqlCommand時，連同資料庫名稱都要寫上去
            Cmd.CommandText = "select p.PhotoID,p.title,c.subject,c.body,c.UserName from PhotoSharingEntities.Photos as p inner join PhotoSharingEntities.comments as c on p.PhotoID=c.PhotoID";

            Conn.Open();
            var rd = Cmd.ExecuteReader(CommandBehavior.SequentialAccess);

            //不一定要arraylist來接資料
            ArrayList list = new ArrayList();
            while (rd.Read())
            {
                var data = new
                {
                    PhotoID = rd["PhotoID"].ToString(),
                    title = rd["title"].ToString(),
                    subject = rd["subject"].ToString(),
                    body = rd["body"].ToString(),
                    UserName = rd["UserName"].ToString()
                };
                list.Add(data);
            }
            Conn.Close();
            ViewBag.Data = list;

            return View();
        }
        public ActionResult CreateComment(int? id) {

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.PhotoID = id;
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult CreateComment(Comments comments)
        {
            Conn.ConnectionString = "Name=PhotoSharingEntities";
            var Cmd = Conn.CreateCommand();
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "PhotoSharingEntities.Add_Comment";

            Cmd.Parameters.AddWithValue("username",comments.UserName);
            Cmd.Parameters.AddWithValue("subject",comments.Subject);
            Cmd.Parameters.AddWithValue("body",comments.Body);
            Cmd.Parameters.AddWithValue("photoid",comments.PhotoID);

            Conn.Open();
            Cmd.ExecuteNonQuery();
            Conn.Close();


            return RedirectToAction("Index");
        }
    }
}