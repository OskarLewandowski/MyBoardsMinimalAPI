using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Value { get; set; }

        //relations many-to-many  WorkItem--[*]----[*]--Tag
        //public List<WorkItemTag> WorkItemTags { get; set; } = new List<WorkItemTag>();
        public virtual List<WorkItem> WorkItems { get; set; } 
    }
}
