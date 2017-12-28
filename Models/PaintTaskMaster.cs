using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class PaintTaskMaster:BaseModel
    {
        [Key]
        public int PaintTaskMasterId { get; set; }

        [StringLength(50)]
        public string TaskPaintNo { get; set; }
        [Required]
        public DateTime? AssignDate { get; set; }
        public string AssignBy { get; set; }
        public double? MainProgress { get; set; }
        [Required]
        public PaintTaskStatus PaintTaskStatus { get; set; }

        //FK
        // RequirePaintingList
        public int? RequirePaintingListId { get; set; }
        public RequirePaintingList RequirePaintingList { get; set; }
        // PaintTaskDetail
        public ICollection<PaintTaskDetail> PaintTaskDetails { get; set; }

    }

    public enum PaintTaskStatus
    {
        Waiting = 1,
        Complated = 2,
        Cancel = 3
    }
}
