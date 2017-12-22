using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class RequisitionMaster:BaseModel
    {
        [Key]
        public int RequisitionMasterId { get; set; }
        public DateTime? RequisitionDate { get; set; }
        [StringLength(50)]
        public string RequisitionBy { get; set; }
        public double? Quantity { get; set; }

        //FK
        //ColorItem
        public int? ColorItemId { get; set; }
        public ColorItem ColorItem { get; set; }
        //TaskPaintDetail
        public int? TaskPaintDetailId { get; set; }
        public TaskPaintDetail TaskPaintDetail { get; set; }
    }
}
