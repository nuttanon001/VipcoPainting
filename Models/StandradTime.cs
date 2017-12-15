using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoPainting.Models
{
    public class StandradTime:BaseModel
    {
        [Key]
        public int StandradTimeId { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        public TypeStandardTime? TypeStandardTime { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public double? Rate { get; set; }
        [StringLength(50)]
        public string RateUnit { get; set; }
        public double? PercentLoss { get; set; }

        // FK
        // RequirePaintingSub
        public ICollection<RequirePaintingSub> RequirePaintingSubs { get; set; }
    }

    public enum TypeStandardTime
    {
        Paint = 1,
        Blast = 2
    }
}
