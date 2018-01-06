using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class MovementStockStatus:BaseModel
    {
        [Key]
        public int MovementStockStatusId { get; set; }
        [StringLength(50)]
        public string StatusName { get; set; }
        public StatusMovement StatusMovement { get; set; }
        public TypeStatusMovement TypeStatusMovement { get; set; }

        //FK
        //ColorMovementStock
        public ICollection<ColorMovementStock> ColorMovementStocks { get; set; }
    }

    public enum StatusMovement
    {
        Stock = 1,
        Requsition = 2,
        AdjustIncreased = 3,
        AdjustDecreased = 4,
        Cancel = 5
    }

    public enum TypeStatusMovement
    {
        Increased = 1,
        Decreased = 2
    }
}
