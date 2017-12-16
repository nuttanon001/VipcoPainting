using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoPainting.Models
{
    public class ColorItem: BaseModel
    {
        [Key]
        public int ColorItemId { get; set; }
        [StringLength(50)]
        public string ColorCode { get; set; }
        [StringLength(200)]
        public string ColorName { get; set; }
        public double? VolumeSolids { get; set; }

        //FK
    }
}
