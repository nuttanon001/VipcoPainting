using System;
using System.Collections.Generic;

namespace VipcoPainting.Models
{
    public partial class ProjectCodeMaster
    {
        public ProjectCodeMaster()
        {
            ProjectCodeDetail = new HashSet<ProjectCodeDetail>();
        }

        public int ProjectCodeMasterId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Creator { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string Modifyer { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public DateTime? StartDate { get; set; }

        public ICollection<ProjectCodeDetail> ProjectCodeDetail { get; set; }
    }
}
