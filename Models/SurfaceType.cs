using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoPainting.Models
{
    public class SurfaceType:BaseModel
    {
        [Key]
        public int SurfaceTypeId { get; set; }
        [StringLength(100)]
        public string SurfaceCode { get; set; }
        [StringLength(200)]
        public string SurfaceName { get; set; }

        // FK
        // RequirePaintingSub
        public ICollection<RequirePaintingSub> RequirePaintingSubs { get; set; }
    }
}
