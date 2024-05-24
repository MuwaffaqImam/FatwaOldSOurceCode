using System;
using System.Collections.Generic;

namespace TeamWork.Core.Models.Fatawa
{
    public class FatawaExportModel
    {
        public int? FatwaId { get; set; }
        public string Name { get; set; }
        public string FatawaQuestion { get; set; }
        public string Description { get; set; }
        public string MuftiName { get; set; }
        public int FatawaTypeName { get; set; }
        public string FatawaDepartmentName { get; set; }
        public string MathhabName { get; set; }
    }
}
