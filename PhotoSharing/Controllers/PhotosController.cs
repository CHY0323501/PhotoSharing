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
            return View(context.Photos.ToList());
        }
        public ActionResult Display(int? id)
        {
            ViewData["Date"] = DateTime.Now;    //mvc4帶資料的方式，viewbag為mvc5帶資料新方式
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Photo photo = context.Photos.Find(id);
            if (photo == null) {
                return HttpNotFound();
            }
            
            return View(photo);
        }
        public ActionResult Create() {
            Photo newPhoto = new Photo();
            newPhoto.CreatedDate = DateTime.Today;
            return View(newPhoto);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Photo photo ,HttpPostedFileBase img)
        {
            photo.CreatedDate = DateTime.Today;
            if (ModelState.IsValid)
            {
                photo.ImageMimeType = img.ContentType;      //抓照片副檔名
                photo.PhotoFile = new byte[img.ContentLength];
                img.InputStream.Read(photo.PhotoFile, 0, img.ContentLength);
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

        //取得照片
        public FileContentResult GetImage(int id) {
            Photo photo= context.Photos.Find(id);

            return File(photo.PhotoFile, photo.ImageMimeType);
        }
    }
}