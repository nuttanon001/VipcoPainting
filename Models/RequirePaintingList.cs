﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VipcoPainting.Models
{
    public class RequirePaintingList
    {
        [Key]
        public int RequirePaintingListId { get; set; }
        [Required]
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(150)]
        public string PartNo { get; set; }
        [StringLength(150)]
        public string MarkNo { get; set; }
        [StringLength(150)]
        public string DrawingNo { get; set; }
        public int? UnitNo { get; set; }
        public double? Quantity { get; set; }
        public bool? FieldWeld { get; set; }
        public bool? Insulation { get; set; }
        public bool? ITP { get; set; }
        public double? SizeL { get; set; }
        public double? SizeW { get; set; }
        public double? SizeH { get; set; }
        public double? Weight { get; set; }

        [StringLength(50)]
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string Modifyer { get; set; }
        public DateTime? ModifyDate { get; set; }

        //FK
        // RequirePaintingMaster
        public int? RequirePaintingMasterId { get; set; }
        public RequirePaintingMaster RequirePaintingMaster { get; set; }
        // RequirePaintingSub
        public ICollection<RequirePaintingSub> RequirePaintingSubs { get; set; }
    }
}
