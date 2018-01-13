using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class SubPaymentDetail:BaseModel
    {
        [Key]
        public int SubPaymentDetailId { get; set; }
        public double? AreaWorkLoad { get; set; }
        public double? CalcCost { get; set; }
        public DateTime? PaymentDate { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        //FK
        //SubPaymentMaster
        public int? SubPaymentMasterId { get; set; }
        public SubPaymentMaster SubPaymentMaster { get; set; }
        //PaintTaskDetail
        public int? PaintTaskDetailId { get; set; }
        public PaintTaskDetail PaintTaskDetail { get; set; }
        //PaymentCostHistory
        public int? PaymentCostHistoryId { get; set; }
        public PaymentCostHistory PaymentCostHistory { get; set; }
    }
}
