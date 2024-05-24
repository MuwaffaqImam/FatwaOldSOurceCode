using System;
using System.Collections.Generic;

namespace TeamWork.Core.Models.Fatawa
{
    public class FatawaImportModel : BaseModel
    {

        public int fatawaMathhabId { get; set; }

        public int fatawaDepartmentId { get; set; }

        public int muftiId { get; set; }
        public int fatawaTypeId { get; set; }


    }

}
