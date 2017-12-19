using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoPainting.Models
{
    public class BlastWorkItem:BaseModel
    {
        [Key]
        public int BlastWorkItemId { get; set; }
        public double? IntArea { get; set; }
        public double? IntCalcStdUsage { get; set; }

        public double? ExtArea { get; set; }
        public double? ExtCalcStdUsage { get; set; }
        //FK

        // StandradTimeInt
        public int? StandradTimeIntId { get; set; }

        [ForeignKey("StandradTimeIntId")]
        public StandradTime StandradTimeInt { get; set; }

        // StandradTimeInt
        public int? StandradTimeExtId { get; set; }

        [ForeignKey("StandradTimeExtId")]
        public StandradTime StandradTimeExt { get; set; }

        // SurfaceTypeInt
        public int? SurfaceTypeIntId { get; set; }

        [ForeignKey("SurfaceTypeIntId")]
        public SurfaceType SurfaceTypeInt { get; set; }

        // SurfaceTypeExt
        public int? SurfaceTypeExtId { get; set; }

        [ForeignKey("SurfaceTypeExtId")]
        public SurfaceType SurfaceTypeExt { get; set; }

        // RequirePaintingList 
        public int? RequirePaintingListId { get; set; }
        public RequirePaintingList RequirePaintingList { get; set; }
    }
}
