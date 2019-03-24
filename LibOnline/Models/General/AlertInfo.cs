using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibOnline.Models.General
{
    public class AlertInfo
    {
        public string ResponseText { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsAlert { get; set; }

        public AlertInfo()
        {
            IsAlert = true;
        }
    }
}
