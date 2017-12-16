using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VipcoPainting.Models
{
    public class RequirePaintingSub:BaseModel
    {
        [Key]
        public int RequirePaintingSubId { get; set; }
        public PaintingArea? PaintingArea { get; set; }
        public PaintingType? PaintingType { get; set; }
        public double? Area { get; set; }
        public double? DFTMin { get; set; }
        public double? DFTMax { get; set; }
        public double? CalcColorUsage { get; set; }
        public double? CalcStdUsage { get; set; }

        //FK
        // ColorItem
        public int? ColorItemId { get; set; }
        public ColorItem ColorItem { get; set; }
        // SurfaceType
        public int? SurfaceTypeId { get; set; }
        public SurfaceType SurfaceType { get; set; }
        // StandradTime
        public int? StandradTimeId { get; set; }
        public StandradTime StandradTime { get; set; }
        // RequirePaintingList 
        public int? RequirePaintingListId { get; set; }
        public RequirePaintingList RequirePaintingList { get; set; }
    }

    public enum PaintingArea
    {
        Internal = 1,
        External = 2
    }

    public enum PaintingType
    {
        PrimerCoat = 1,
        MidCoat = 2,
        IntermediateCoat = 3,
        TopCoat = 4,
        Blasting = 5
    }
}
