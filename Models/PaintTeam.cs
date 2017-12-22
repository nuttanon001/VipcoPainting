using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class PaintTeam:BaseModel
    {
        [Key]
        public int PaintTeamId { get; set; }
        [StringLength(100)]
        public string TeamName { get; set; }
        [StringLength(200)]
        public string Ramark { get; set; }
        
        //FK
        // BlastRoom
        public ICollection<BlastRoom> BlastRooms { get; set; }
        // TaskPaintDetail
        public ICollection<TaskPaintDetail> TaskPaintDetails { get; set; }
    }
}
