using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class PaymentDetail : BaseModel
    {
        [Key]
        public int PaymentDetailId { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public double LastCost { get; set; }
        public PaymentType PaymentType { get; set; }
        //FK
        //PaymentCostHistory
        public ICollection<PaymentCostHistory> PaymentCostHistorys { get; set; }
        //PaintTaskDetail
        public ICollection<PaintTaskDetail> PaintTaskDetails { get; set; }
        //SubPaymentDetail
        public ICollection<SubPaymentDetail> SubPaymentDetails { get; set; }
    }

    public enum PaymentType
    {
        Blast = 1,
        Paint = 2
    }
}
