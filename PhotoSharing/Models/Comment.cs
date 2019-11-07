using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoSharing.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "不可超過100字")]
        [DisplayName("回覆主旨")]
        public string Subject { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "不可超過400字")]
        [DisplayName("回覆內容")]
        public string Body { get; set; }
        [Required]
        [DisplayName("回覆人")]
        public string UserName { get; set; }

        //建立關聯(1:N，需在多的資料表中寫關聯)
        public virtual Photo Photos { get; set; }
        //與哪個欄位建立關聯
        public int PhotoID { get; set; }
    }
}