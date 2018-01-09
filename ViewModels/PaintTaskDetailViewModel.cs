using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using VipcoPainting.Models;

namespace VipcoPainting.ViewModels
{
    public class PaintTaskDetailViewModel:PaintTaskDetail
    {
        public string PaintTeamString { get; set; }
        public string BlastRoomString { get; set; }
        public string CommonText { get; set; }
        public double SummaryActual { get; set; }

    }
}
