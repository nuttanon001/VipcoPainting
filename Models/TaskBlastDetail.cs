using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace VipcoPainting.Models
{
    public class TaskBlastDetail:BaseModel
    {
        [Key]
        public int TaskBlastDetailId { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }

        //FK
        //TaskMaster
        public int? TaskMasterId { get; set; }
        public TaskMaster TaskMaster { get; set; }
        //BlastRoom
        public int? BlastRoomId { get; set; }
        public BlastRoom BlastRoom { get; set; }
        //BlastWorkItem
        public int? BlastWorkItemId { get; set; }
        public BlastWorkItem BlastWorkItem { get; set; }
    }
}
