using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoPainting.Models
{
    public class RequirePaintingMaster:BaseModel
    {
        [Key]
        public int RequirePaintingMasterId { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public DateTime? RequireDate { get; set; }
        public DateTime? FinishDate { get; set; }
        [StringLength(200)]
        public string PaintingSchedule { get; set; }
        [StringLength(200)]
        public string RequireNo { get; set; }

        //FK
        // RequireEmployee
        public string RequireEmp { get; set; }
        // ReciveEmployee
        public string ReceiveEmp { get; set; }
        // ProjectCodeSub
        public int? ProjectCodeSubId { get; set; }
        public ProjectCodeSub ProjectCodeSub { get; set; }
        // RequirePaintingLists
        public ICollection<RequirePaintingList> RequirePaintingLists { get; set; }

    }
}
