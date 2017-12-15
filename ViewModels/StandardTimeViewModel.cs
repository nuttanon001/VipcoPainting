using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using VipcoPainting.Models;
namespace VipcoPainting.ViewModels
{
    public class StandardTimeViewModel:StandradTime
    {
        public string RateWithUnit { get; set; }
        public string PercentLossString { get; set; }
        public string TypeStandardTimeString { get; set; }
    }
}
