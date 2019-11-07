using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using namespace
using System.Data.Entity;
using System.IO;

namespace PhotoSharing.Models
{
    //創建資料庫時一並匯入資料
    
    public class PhotoSharingInitialier:DropCreateDatabaseAlways<PhotoSharingContext>   //角括號內放dbcontext(資料庫連線資訊)
    {
        protected override void Seed(PhotoSharingContext context)
        {
            base.Seed(context);
            List<Photo> photos = new List<Photo>
            {
                 new Photo
                {
                   Title="Me standing on top of a mountain",
                    Description="I was very impressed with myself",
                    PhotoFile=getFileBytes("\\Images\\img1.jpg"),
                    ImageMimeType="image/jpeg",
                    CreatedDate=DateTime.Today,
                    UserName="Fred"


                },
               new Photo {
                    Title = "My New Adventure Works Bike",
                    Description = "It's the bees knees!",
                    UserName = "Fred",
                    PhotoFile = getFileBytes("\\Images\\img2.jpg"),
                    ImageMimeType = "image/jpeg",
                    CreatedDate = DateTime.Today
                },
                new Photo {
                    Title = "View from the start line",
                    Description = "I took this photo just before we started over my handle bars.",
                    UserName = "Sue",
                    PhotoFile = getFileBytes("\\Images\\img3.jpg"),
                    ImageMimeType = "image/jpeg",
                    CreatedDate = DateTime.Today
                },
                new Photo {
                    Title = "Sample Beauty Flower",
                    Description = "This is a Sample flower",
                    UserName = "Lee",
                    PhotoFile = getFileBytes("\\Images\\img4.jpg"),
                    ImageMimeType = "image/jpeg",
                    CreatedDate = DateTime.Today
                }

            };

            photos.ForEach(s => context.Photos.Add(s));
            context.SaveChanges();
        }
        private byte[] getFileBytes(string path) {

            FileStream fileOnDisk = new FileStream(HttpRuntime.AppDomainAppPath + path, FileMode.Open);
            byte[] fileBytes;
            using (BinaryReader br = new BinaryReader(fileOnDisk)) {
                fileBytes = br.ReadBytes((int)fileOnDisk.Length);
            }
            return fileBytes;

        }
    }

}