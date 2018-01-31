using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VipcoPainting.ViewModels
{
    public class OptionTaskMasterSchedule
    {
        public string Filter { get; set; }
        public int? ProjectMasterId { get; set; }
        public int? ProjectSubId { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        /// <summary>
        /// 1 : All Task Without cancel
        /// null or 2 : Wait and Process only
        /// </summary>
        public int? Mode { get; set; }
        public string Creator { get; set; }
        public int? TaskMasterId { get; set; }
        /// <summary>
        /// PaintTeam
        /// </summary>
        public int? PaintTeamId { get; set; }
        ///<summary>
        ///BlastTeam
        /// </summary>
        public int? BlastTeamId { get; set; }
        /// <summary>
        /// Start date
        /// </summary>
        public DateTime? SDate { get; set; }
        /// <summary>
        /// End date
        /// </summary>
        public DateTime? EDate { get; set; }
    }
}
