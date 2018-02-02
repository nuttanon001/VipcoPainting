using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class InitialRequirePaintingList:BaseModel
    {
        [Key]
        public int InitialRequireId { get; set; }
        public DateTime? PlanStart { get; set; }
        public DateTime? PlanEnd { get; set; }
        public string DrawingNo { get; set; }
        public int? UnitNo { get; set; }
        //FK
        // RequirePaintingMaster
        public int? RequirePaintingMasterId { get; set; }
        public RequirePaintingMaster RequirePaintingMaster { get; set; }
        // BlastWorkItem
        public ICollection<BlastWorkItem> BlastWorkItems { get; set; }
        // PaintWorkItem
        public ICollection<PaintWorkItem> PaintWorkItems { get; set; }
    }
}
