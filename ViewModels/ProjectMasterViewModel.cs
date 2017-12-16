using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VipcoPainting.Models;

namespace VipcoPainting.ViewModels
{
    public class ProjectMasterViewModel:ProjectCodeMaster
    {
        public List<ProjectCodeSub> ProjectSubs { get; set; }
    }
}
