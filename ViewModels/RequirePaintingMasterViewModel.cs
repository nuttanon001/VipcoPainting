using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoPainting.Models;

namespace VipcoPainting.ViewModels
{
    public class RequirePaintingMasterViewModel:RequirePaintingMaster
    {
        public string JobCode { get; set; }
        public string RequireString { get; set; }
        public string ProjectCodeSubString { get; set; }
        public int? InitialRequireId { get; set; }
    }
}
