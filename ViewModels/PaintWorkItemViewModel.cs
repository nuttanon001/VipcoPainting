using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoPainting.Models;

namespace VipcoPainting.ViewModels
{
    public class PaintWorkItemViewModel:PaintWorkItem
    {
        public string IntColorString { get; set; }
        public string ExtColorString { get; set; }
        public string IntStandradTimeString { get; set; }
        public string ExtStandradTimeString { get; set; }
        public string PaintLevelString { get; set; }
    }
}
