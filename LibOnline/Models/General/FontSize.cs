using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Models.General
{
    [Table("PageFontSizes", Schema = "general")]
    public class PageFontSizes
    {
        [Key]
        public int IdPageFontSize { get; set; }
        public string FontSizeValue { get; set; }
        public bool IsMain { get; set; }
    }//FontSize
}
