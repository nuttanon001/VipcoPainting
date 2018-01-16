using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoPainting.Models
{
    public class SubPaymentMaster : BaseModel
    {
        [Key]
        public int SubPaymentMasterId { get; set; }
        [StringLength(100)]
        public string SubPaymentNo { get; set; }
        public DateTime? SubPaymentDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubPaymentMasterStatus? SubPaymentMasterStatus { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        [StringLength(50)]
        public string EmpApproved1 { get; set; }
        [StringLength(50)]
        public string EmpApproved2 { get; set; }
        //FK
        //SubPaymentMaster
        public int? PrecedingSubPaymentId { get; set; }
        [ForeignKey("PrecedingSubPaymentId")]
        public SubPaymentMaster PrecedingSubPayment { get; set; }
        //ProjectMaster
        public int? ProjectCodeMasterId { get; set; }
        //PaintTeam
        public int? PaintTeamId { get; set; }
        public PaintTeam PaintTeam { get; set; }
        //SubPaymentDetal
        public ICollection<SubPaymentDetail> SubPaymentDetails { get; set; }
    }

    public enum SubPaymentMasterStatus
    {
        Waiting = 1,
        Complate = 2,
        Cancel = 3
    }
}
