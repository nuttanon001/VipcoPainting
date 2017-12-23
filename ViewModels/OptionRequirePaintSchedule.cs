using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VipcoPainting.ViewModels
{
    public class OptionRequirePaintSchedule
    {
        public string Filter { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        /// <summary>
        /// Required = 1,
        /// WaitActual = 2,
        /// Complate = 3,
        /// Cancel = 4
        /// </summary>
        public int? Status { get; set; }
    }
}
