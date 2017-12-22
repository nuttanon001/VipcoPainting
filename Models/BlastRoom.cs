using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class BlastRoom:BaseModel
    {
        [Key]
        public int BlastRoomId { get; set; }
        [StringLength(100)]
        public string BlastRoomName { get; set; }
        [MinLength(1)]
        [MaxLength(100)]
        public int? BlastRoomNumber { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        //FK
        // PaintTeam
        public int? PaintTeamId { get; set; }
        public PaintTeam PaintTeam { get; set; }
        //TaskBlastDetail
        public ICollection<TaskBlastDetail> TaskBlastDetails { get; set; }
    }
}
