using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoSharing.Models
{
    public class Photo
    {
        //codefirst
        //PhotoID is the primary key
        public int PhotoID { get; set; }
        [Required]
        [StringLength(150,ErrorMessage ="不可超過150字")]
        [DisplayName("主題")]
        public string Title { get; set; }
        [MaxLength]
        [DisplayName("上傳圖片")]
        public byte[] PhotoFile{ get; set; }
        [HiddenInput(DisplayValue =false)]
        public string ImageMimeType{ get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "不可超過500字")]
        [DisplayName("描述")]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd}",ApplyFormatInEditMode =true)]
        [DisplayName("發表日期")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [CheckUsername] //自訂驗證器(需繼承ValidationAttribute類別)，當結果為false時表單無法送出
        [DisplayName("發表人")]
        public string UserName { get; set; }

    }
    public class CheckUsername : ValidationAttribute {
        public CheckUsername() {
            ErrorMessage = "發表人名稱至少三個字";
        }
        public override bool IsValid(object value)
        {
            return (value.ToString().Length >= 3) ? true : false;
        }
    }
}