using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoPainting.Models;

namespace VipcoPainting.ViewModels
{
    public class SubPaymentMasterViewModel: SubPaymentMaster
    {
        public string EmpApproved1String { get; set; }
        public string EmpApproved2String { get; set; }
        public string ProjectCodeMasterString { get; set; }
        public string PaintTeamString { get; set; }
        public string SubPaymentMasterStatusString { get; set; }
    }
}
