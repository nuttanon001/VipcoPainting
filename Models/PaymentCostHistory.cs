using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class PaymentCostHistory : BaseModel
    {
        [Key]
        public int PaymentCostHistoryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double PaymentCost { get; set; }
        
        //FK
        //PaymentDetail
        public int PaymentDetailId { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
        //SubPaymentDetail
        public ICollection<SubPaymentDetail> SubPaymentDetails { get; set; }
    }
}
