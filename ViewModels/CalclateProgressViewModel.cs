using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoPainting.ViewModels
{
    public class CalclateProgressViewModel
    {
        public int? PaymentDetailId { get; set; }
        public string Description { get; set; }
        public double LastCost { get; set; }
        public int? PaintTaskDetailId { get; set; }
        public double? AreaBlastIn { get; set; }
        public double? AreaBlastEx { get; set; }
        public double? AreaPaintIn { get; set; }
        public double? AreaPaintEx { get; set; }
    }
}
