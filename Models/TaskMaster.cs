using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class TaskMaster : BaseModel
    {
        [Key]
        public int TaskMasterId { get; set; }

        [StringLength(50)]
        public string TaskNo { get; set; }

        [Required]
        public DateTime? AssignDate { get; set; }
        public string AssignBy { get; set; }
        public DateTime? ActualSDate { get; set; }
        public DateTime? ActualEDate { get; set; }
        public double? TaskProgress { get; set; }

        [Required]
        public TaskStatus? TaskStatus { get; set; }

        //FK
        // RequirePaintingList
        public int? RequirePaintingListId { get; set; }

        public RequirePaintingList RequirePaintingList { get; set; }
        // TaskBlastDetail
        public ICollection<TaskBlastDetail> TaskBlastDetails { get; set; }
        // TaskPaintDetail
        public ICollection<TaskPaintDetail> TaskPaintDetails { get; set; }
    }

    public enum TaskStatus
    {
        Waiting = 1,
        Tasking = 2,
        Complated = 3,
        Cancel = 4
    }
}