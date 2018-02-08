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
        public double? AreaCodition { get; set; }
        public Codition? Codition { get; set; }

        // FK
        //StandardTime
        public int? LinkStandardTimeId { get; set; }
        [ForeignKey("LinkStandardTimeId")]
        public StandradTime LinkStandradTime { get; set; }
    }

    public enum TypeStandardTime
    {
        Paint = 1,
        Blast = 2
    }

    public enum Codition
    {
        Equal = 1,
        Over = 2,
        Less = 3,
        None = 4,
    }
}
