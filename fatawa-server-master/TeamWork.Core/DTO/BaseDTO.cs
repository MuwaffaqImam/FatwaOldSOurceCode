using System;
using System.Collections.Generic;
using System.Text;

namespace TeamWork.Core.DTO
{
    public class BaseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
