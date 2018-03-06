using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoPainting.Models;

namespace VipcoPainting.ViewModels
{
    public class RequirePaintingListViewModel:RequirePaintingList
    {
        public bool IsReceive { get; set; }
        public double IntArea { get; set; }
        public double ExtArea { get; set; }
    }
}
