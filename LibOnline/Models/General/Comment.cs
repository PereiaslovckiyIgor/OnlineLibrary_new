using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Models.General
{
    public class Comment
    {
        [Key]
        public int IdComment { get; set; }
        public int IdBook { get; set; }
        public string Login { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentText { get; set; }
        public bool IsPuplic { get; set; }


        public string MonthRU()
        {
            return CommentDate.ToString("MMMM", CultureInfo.GetCultureInfo("ru-RU"));
        }
    }//Comment

}//LibOnline
