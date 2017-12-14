using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoPainting.Models
{
    public class ProjectCodeSub
    {
        [Key]
        public int ProjectCodeSubId { get; set; }
        [StringLength(50)]
        public string Code { get; set; }
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }
        // FK
        //ProjectCodeSub
        public int? ProjectSubParentId { get; set; }
        [ForeignKey("ProjectSubParentId")]
        public ProjectCodeSub ProjectSubParent { get; set; }
        //ProjectMasterCode
        public int? ProjectCodeMasterId { get; set; }
        public ProjectCodeMaster ProjectCodeMaster { get; set; }
    }
}
