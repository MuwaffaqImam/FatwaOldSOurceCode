using System;
using System.Collections.Generic;

namespace TeamWork.Core.Models.Security
{
    public class UsertreeModel : BaseModel
    {
        public int ParentId { get; set; }
        public string NodeNumber { get; set; }
        public string NodeMain { get; set; }
        public int NodeLevelNumber { get; set; }
        public string NodeName { get; set; }
        public int mufitUserId { get; set; }
        public int Sort { get; set; }
    }
}
