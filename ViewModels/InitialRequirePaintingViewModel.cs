using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoPainting.Models;

namespace VipcoPainting.ViewModels
{
    public class InitialRequirePaintingViewModel:InitialRequirePaintingList
    {
        public bool NeedInternal { get; set; }
        public bool NeedExternal { get; set; }
    }
}
