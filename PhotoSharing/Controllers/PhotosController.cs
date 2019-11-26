using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PhotoSharing.Models;

namespace PhotoSharing.Controllers
{
    public class PhotosController : Controller
    {
        PhotoSharingContext context = new PhotoSharingContext();
        // GET: Photos
        public ActionResult Index()
        {
            ViewBag.Date = DateTime.Now;    //把現在的時間丟到viewbag
            //return View(context.Photos.ToList());
            return View();
        }
        public ActionResult Display(int? id)
        {
            ViewData["Date"] = DateTime.Now;    //mvc4帶資料的方式，viewbag為mvc5帶資料新方式，viewdata型別需要轉型、viewbag不需要
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Photo photo = context.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            return View(photo);
        }
        public ActionResult Create()
        {
            Photo newPhoto = new Photo();
            newPhoto.CreatedDate = DateTime.Today;
            return View(newPhoto);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Photo photo, HttpPostedFileBase img)
        {
            photo.CreatedDate = DateTime.Today;
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    photo.ImageMimeType = img.ContentType;      //抓照片副檔名
                    photo.PhotoFile = new byte[img.ContentLength];
                    img.InputStream.Read(photo.PhotoFile, 0, img.ContentLength);
                };
                context.Photos.Add(photo);
                context.SaveChanges();
                return RedirectToAction("index");
            }
            return View(photo);
        }
        //刪除確認頁
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Photo photo = context.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            return View(photo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            Photo photo = context.Photos.Find(id);
            context.Photos.Remove(photo);
            context.SaveChanges();
            return RedirectToAction("index");
        }
        //partial view需標以下標籤
        //partial view名稱底線開頭表示禁止單獨存取此view，需有其他主view才可存取
        [ChildActionOnly]
        public ActionResult _PhotoGallery(int number = 0)
        {
            List<Photo> photos;
            if (number == 0)
            {
                photos = context.Photos.OrderByDescending(m => m.CreatedDate).ThenByDescending(m => m.PhotoID).ToList();
            }
            else
            {
                photos = context.Photos.OrderByDescending(m => m.CreatedDate).ThenByDescending(m => m.PhotoID).Take(number).ToList();
                //相當於SQL
                //select top 2 * from photos order by CreateDate,PhotoID
            }
            return PartialView(photos);
        }
        //發生例外顯示的view
        public ActionResult ExceptionDemo() {
            int i = 0;
            int j = 10 / i;

            return View();
        }

        //若發生400或500系列錯誤(伺服器層級錯誤)要指定特定page
        //需到webconfig中的<system.webServer>中輸入以下程式碼
            //<httpErrors errorMode = "Custom" >
            //<remove statusCode="404"/>
            //<error statusCode = "404" path="/404.html" responseMode="ExecuteURL"/>
            //<remove statusCode = "400" />
            //<error statusCode="400" path="/400.html" responseMode="ExecuteURL"/>
            //</httpErrors>


        //發生例外顯示的view2
        //尚未實作的例外(throw new NotImplementedException)
        //1.要到app_start的filterconfig中先註冊filters.Add(new HandleErrorAttribute());
        //2.到webconfig中的system.web中新增<customErrors mode="On"></customErrors>，mode="remoteonly"表示開發人員看到的是系統錯誤頁面，使用者看到的是導到自訂的error頁面
        //要指定相對的error page，使用負面表列方式(因為filter已註冊整個應用程式層級的錯誤訊息註冊)，加上以下標籤
        [HandleError(View ="ExceptionError")]
        public ActionResult SliderShow()
        {
            throw new NotImplementedException("這一個相簿功能尚未完成");
        }

        //取得照片
        //負面表列(註冊應用程式紀錄後，排除少數不紀錄)
        [ValueReporter(IsLog = false)]
        public FileContentResult GetImage(int id)
        {
            Photo photo = context.Photos.Find(id);
            if (photo != null)
                return File(photo.PhotoFile, photo.ImageMimeType);
            else
                return null;
        }

        //路由可帶文字
        [Route(@"photos/title/{title}")]//雙引號前加上@
        public ActionResult DisplayByTitle(string title) {
            Photo photo = context.Photos.Where(m => m.Title == title).FirstOrDefault();
            if (photo == null)
                HttpNotFound();
            return View("Display",photo);
        }

        public ActionResult AjaxDemo() {
            return View();
        }
        //AjaxDemo
        public ActionResult JsonData(string id) {
            if (Request.IsAjaxRequest())
            {
                Photo photo = context.Photos.Find(int.Parse(id));
                if (photo == null)
                    return HttpNotFound();
                var data = new
                {
                    id = photo.PhotoID,
                    title = photo.Title,
                    description=photo.Description,
                    createdate=photo.CreatedDate
                };
                return Json(data);
            };
            return View();
        }
    }
}


//更新資料庫
//      做法1:重新執行Initializer
//      做法2:[工具]→[NuGet套件管理員]→[套件管理主控台], 輸入以下指令
//      Enable-Migrations, 按Enter
//      Add-Migration XXXXX, 按Enter
//      Update-Database, 按Enter

//Transaction
//create proc Add_Comment
//    @username nvarchar(100),
//	@subject nvarchar(250),
//	@body nvarchar(max),
//	@photoid int
//as
//begin
//    insert into Comments(username, [subject], body, photoid)

//    values(@username, @subject, @body, @photoid)


//end