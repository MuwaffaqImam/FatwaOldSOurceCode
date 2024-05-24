using System;
using System.Collections.Generic;
using System.Text;

namespace TeamWork.Core.Models.DragDrop
{
    public class DragDropModel
    {
        public int PreviousNodeId { get; set; }
        public int CurrentNodeId { get; set; }
        public bool IsMoveToDown { get; set; }

    }
}
