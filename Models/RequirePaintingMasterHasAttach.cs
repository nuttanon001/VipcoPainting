using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VipcoPainting.Models
{
    public class RequirePaintingMasterHasAttach:BaseModel
    {
        [Key]
        public int RequirePaintingMasterHasAttachId { get; set; }

        //FK
        // RequirePaintingList
        public int? RequirePaintingMasterId { get; set; }
        public RequirePaintingMaster RequirePaintingMaster { get; set; }

        // AttachFile
        public int? AttachFileId { get; set; }
    }
}
