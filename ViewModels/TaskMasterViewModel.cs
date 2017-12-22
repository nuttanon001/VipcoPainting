using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using VipcoPainting.Models;

namespace VipcoPainting.ViewModels
{
    public class TaskMasterViewModel:TaskMaster
    {
        public string AssignByString { get; set; }
        public string ProjectCodeSubString { get; set; }
    }
}
