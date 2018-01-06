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
        [StringLength(200)]
        public string Remark { get; set; }

        //FK
        //ColorItem
        public int? ColorItemId { get; set; }
        public ColorItem ColorItem { get; set; }
        //PaintTaskDetail
        public int? PaintTaskDetailId { get; set; }
        public PaintTaskDetail PaintTaskDetail { get; set; }
        //ColorMovementStock
        public int? ColorMovementStockId { get; set; }
        public ColorMovementStock ColorMovementStock { get; set; }
    }
}
