using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoSharing.Models;


namespace PhotoSharing.Controllers
{
    public class CommentsController : Controller
    {
        PhotoSharingContext context = new PhotoSharingContext();
        [ChildActionOnly]
        public PartialViewResult _CommentsForPhoto(int PhotoID)
        {
            var comments = context.Comment.Where(m=>m.PhotoID== PhotoID).ToList();

            ViewBag.PhotoID = PhotoID;


            return PartialView(comments);
        }
        public PartialViewResult _Create(int PhotoID) {
            Comment newComment = new Comment();
            newComment.PhotoID = PhotoID;

            ViewBag.PhotoID = PhotoID;

            return PartialView("_CreateComment");
        }
        [HttpPost,ValidateAntiForgeryToken]
        public PartialViewResult _CommentsForPhoto(int PhotoID,Comment comment)
        {
            context.Comment.Add(comment);
            context.SaveChanges();

            var comments = context.Comment.Where(m => m.PhotoID == PhotoID).OrderByDescending(m=>m.CommentID);

            ViewBag.PhotoID = PhotoID;

            return PartialView("_CommentsForPhoto", comments.ToList());
        }
    }
}