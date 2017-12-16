using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoPainting.Models
{
    public class PaintWorkItem:BaseModel
    {
        [Key]
        public int PaintWorkItemId { get; set; }
        public PaintLevel? PaintLevel { get; set; }
        public double? IntArea { get; set; }
        public double? IntDFTMin { get; set; }
        public double? IntDFTMax { get; set; }
        public double? IntCalcColorUsage { get; set; }
        public double? IntCalcStdUsage { get; set; }

        public double? ExtArea { get; set; }
        public double? ExtDFTMin { get; set; }
        public double? ExtDFTMax { get; set; }
        public double? ExtCalcColorUsage { get; set; }
        public double? ExtCalcStdUsage { get; set; }
        //FK
        // ColorItemInt
        public int? IntColorItemId { get; set; }

        [ForeignKey("IntColorItemId")]
        public ColorItem IntColorItem { get; set; }

        // ColorItemExt
        public int? ExtColorItemId { get; set; }

        [ForeignKey("ExtColorItemId")]
        public ColorItem ExtColorItem { get; set; }

        // StandradTimeInt
        public int? StandradTimeIntId { get; set; }

        [ForeignKey("StandradTimeIntId")]
        public StandradTime StandradTimeInt { get; set; }
        // StandradTimeExt
        public int? StandradTimeExtId { get; set; }

        [ForeignKey("StandradTimeExtId")]
        public StandradTime StandradTimeExt { get; set; }

        // RequirePaintingList 
        public int? RequirePaintingListId { get; set; }
        public RequirePaintingList RequirePaintingList { get; set; }
    }

    public enum PaintLevel
    {
        PrimerCoat = 1,
        MidCoat = 2,
        IntermediateCoat = 3,
        TopCoat = 4,
    }
}
