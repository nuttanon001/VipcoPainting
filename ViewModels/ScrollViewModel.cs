using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VipcoPainting.ViewModels
{
    public class ScrollViewModel
    {
        //The Skip number
        public int? Skip { get; set; }
        //The Take number
        public int? Take { get; set; }
        public string SortField { get; set; }
        public int? SortOrder { get; set; }
        public string Filter { get; set; }
        public bool? Reload { get; set; }
        public string Where { get; set; }
    }
}
