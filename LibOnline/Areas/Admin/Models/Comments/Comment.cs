using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Areas.Admin.Models.Comments
{
    public class Comment
    {
        [Key]
        public int IdComments { get; set; }
        public int IdBook { get; set; }
        public string BookName { get; set; }
        public int IdUser { get; set; }
        public string Login { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsPuplic { get; set; }
        public bool IsVerificated { get; set; }
    }
}
