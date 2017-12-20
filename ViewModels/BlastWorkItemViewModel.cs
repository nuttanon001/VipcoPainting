using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoPainting.Models;

namespace VipcoPainting.ViewModels
{
    public class BlastWorkItemViewModel:BlastWorkItem
    {
        public string IntStandradTimeString { get; set; }
        public string ExtStandradTimeString { get; set; }
        public string IntSurfaceTypeString { get; set; }
        public string ExtSurfaceTypeString { get; set; }
    }
}
