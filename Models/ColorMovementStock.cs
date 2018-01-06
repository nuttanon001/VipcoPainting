using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class ColorMovementStock:BaseModel
    {
        [Key]
        public int ColortMovementStockId { get; set; }
        public DateTime MovementStockDate { get; set; }
        public double Quantity { get; set; }

        // FK
        public int? ColorItemId { get; set; }
        public ColorItem ColorItem { get; set; }
        // MovementStockStatus
        public int? MovementStockStatusId { get; set; }
        public MovementStockStatus MovementStockStatus { get; set; }
        // RequisitionMaster
        public RequisitionMaster RequisitionMaster { get; set; }
        // FinishedGoodsMaster
        public FinishedGoodsMaster FinishedGoodsMaster { get; set; }
    }
}
