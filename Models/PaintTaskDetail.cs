using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class PaintTaskDetail:BaseModel
    {
        [Key]
        public int PaintTaskDetailId { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        public PaintTaskDetailStatus? PaintTaskDetailStatus { get; set; }
        public PaintTaskDetailType? PaintTaskDetailType { get; set; }
        public PaintTaskDetailLayer? PaintTaskDetailLayer { get; set; }
        public DateTime PlanSDate { get; set; }
        public DateTime PlanEDate { get; set; }
        public DateTime? ActualSDate { get; set; }
        public DateTime? ActualEDate { get; set; }
        public double? TaskDetailProgress { get; set; }

        //FK
        //PaintTaskMaster
        public int? PaintTaskMasterId { get; set; }
        public PaintTaskMaster PaintTaskMaster { get; set; }
        //PaintTeam
        public int? PaintTeamId { get; set; }
        public PaintTeam PaintTeam { get; set; }
        //BlastRoom
        public int? BlastRoomId { get; set; }
        public BlastRoom BlastRoom { get; set; }
        //PaintWorkItem 
        public int? PaintWorkItemId { get; set; }
        public PaintWorkItem PaintWorkItem { get; set; }
        //BlastWorkItem
        public int? BlastWorkItemId { get; set; }
        public BlastWorkItem BlastWorkItem { get; set; }
        //PaymentDetail
        public int? PaymentDetailId { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
        //RequisitionMaster 
        public ICollection<RequisitionMaster> RequisitionMasters { get; set; }
    }

    public enum PaintTaskDetailStatus
    {
        Tasking = 1,
        Complated = 2,
        Cancel = 3
    }

    public enum PaintTaskDetailType
    {
        Blast = 1,
        Paint = 2
    }

    public enum PaintTaskDetailLayer
    {
        Internal = 1,
        External = 2,
    }
}
