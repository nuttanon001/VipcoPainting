using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoPainting.ViewModels
{
    public class OptionChartViewModel
    {
        public int? BlastRoomId { get; set; }
        public int? PaintTeamId { get; set; }
        public int? ProjectMasterId { get; set; }
        public int? ProjectSubId { get; set; }
        public DateTime? SelectedDate { get; set; }
        public int? TypeChart { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
