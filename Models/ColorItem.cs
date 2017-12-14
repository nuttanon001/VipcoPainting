using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoPainting.Models
{
    public class ColorItem
    {
        [Key]
        public int ColorItemId { get; set; }
        [StringLength(50)]
        public string ColorCode { get; set; }
        [StringLength(200)]
        public string ColorName { get; set; }
        public double? VolumeSolids { get; set; }

        [StringLength(50)]
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        //FK
        //RequirePaintingSub
        public ICollection<RequirePaintingSub> RequirePaintingSubs { get; set; }
    }
}
