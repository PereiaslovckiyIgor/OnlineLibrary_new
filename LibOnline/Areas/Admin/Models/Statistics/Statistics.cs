using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Areas.Admin.Models.Statistics
{

    public class Statistics
    {
        [Key]
        public int BookCount { get; set; }
        public int AuthorsCount { get; set; }
        public int CommentsCount { get; set; }
        public int UsersCount { get; set; }
    }//Statistics
}
