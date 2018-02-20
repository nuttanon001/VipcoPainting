using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class RequirePaintingListHasAttach:BaseModel
    {
        [Key]
        public int RequirePaintingListHasAttachId { get; set; }

        //FK
        // RequirePaintingList
        public int? RequirePaintingListId { get; set; }
        public RequirePaintingList RequirePaintingList { get; set; }

        // AttachFile
        public int? AttachFileId { get; set; }
    }
}
