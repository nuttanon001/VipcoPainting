using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class TaskPaintDetail:BaseModel
    {
        [Key]
        public int TaskPaintDetailId { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        
        //FK
        //TaskMaster
        public int? TaskMasterId { get; set; }
        public TaskMaster TaskMaster { get; set; }
        //PaintTeam
        public int? PaintTeamId { get; set; }
        public PaintTeam PaintTeam { get; set; }
        //PaintWorkItem 
        public int? PaintWorkItemId { get; set; }
        public PaintWorkItem PaintWorkItem { get; set; }
        //RequisitionMaster 
        public ICollection<RequisitionMaster> RequisitionMasters { get; set; }
    }
}
