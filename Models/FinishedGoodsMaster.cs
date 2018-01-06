using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class FinishedGoodsMaster:BaseModel
    {
        [Key]
        public int FinishedGoodsMasterId { get; set; }
        public DateTime FinishedGoodsDate { get; set; }
        [StringLength(50)]
        public string ReceiveBy { get; set; }
        public double? Quantity { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }

        //FK
        //ColorItem
        public int? ColorItemId { get; set; }
        public ColorItem ColorItem { get; set; }
        //ColorMovementStock
        public int? ColorMovementStockId { get; set; }
        public ColorMovementStock ColorMovementStock { get; set; }
    }
}
