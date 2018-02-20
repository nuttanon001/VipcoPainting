using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace VipcoPainting.Models
{
    public class RequirePaintingListOption : BaseModel
    {
        [Key]
        public int RequirePaintingListOptionId { get; set; }

        public DateTime ReceiveWorkItem { get; set; }

        //FK
        //RequirePaintingList
        public int? RequirePaintingListId { get; set; }
        public RequirePaintingList RequirePaintingList { get; set; }
    }
}
