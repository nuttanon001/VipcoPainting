using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoPainting.Models
{
    public class RequirePaintingList : BaseModel
    {
        [Key]
        public int RequirePaintingListId { get; set; }
        public RequirePaintingListStatus? RequirePaintingListStatus { get; set; }
        [Required]
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(150)]
        public string PartNo { get; set; }
        [StringLength(150)]
        public string MarkNo { get; set; }
        [StringLength(150)]
        public string DrawingNo { get; set; }
        public int? UnitNo { get; set; }
        public double? Quantity { get; set; }
        public bool? FieldWeld { get; set; }
        public bool? Insulation { get; set; }
        public bool? ITP { get; set; }
        public double? SizeL { get; set; }
        public double? SizeW { get; set; }
        public double? SizeH { get; set; }
        public double? Weight { get; set; }
        public DateTime? PlanStart { get; set; }
        public DateTime? PlanEnd { get; set; }
        public DateTime? SendWorkItem { get; set; }
        //FK
        // RequirePaintingMaster
        public int? RequirePaintingMasterId { get; set; }
        public RequirePaintingMaster RequirePaintingMaster { get; set; }

        // BlastWorkItem
        public ICollection<BlastWorkItem> BlastWorkItems { get; set; }
        // PaintWorkItem
        public ICollection<PaintWorkItem> PaintWorkItems { get; set; }
    }

    public enum RequirePaintingListStatus
    {
        Waiting = 1,
        Tasking = 2,
        Complate = 3,
        Cancel = 4,
    }
}
