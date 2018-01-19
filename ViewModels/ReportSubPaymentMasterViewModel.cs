using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoPainting.ViewModels
{
    public class ReportSubPaymentMasterViewModel
    {
        public string SubPaymentNo { get; set; }
        public string SubPaymentDate { get; set; }
        public double TotalArea { get; set; }
        public double TotalCost { get; set; }
        public double TotalAllArea { get; set; }
        public double TotalAllCost { get; set; }
        public string Approved1 { get; set; }
        public string Approved2 { get; set; }
        public ICollection<ReportSubPaymentDetailViewModel> Details { get; set; }
    }

    public class ReportSubPaymentDetailViewModel
    {
        public int? RowNumber { get; set; }
        public string Description { get; set; }
        public double? Cost { get; set; }
        public double? AreaWorkLoad { get; set; }
        public double? CalcCost { get; set; }
        public double? AreaTotal { get; set; }
        public double? CostTotal { get; set; }
    }
}
