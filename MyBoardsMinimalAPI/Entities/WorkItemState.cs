using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Entities
{
    public class WorkItemState
    {
        //relations many-to-one  WorkItem--[*]----[1]--WorkItemState
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
