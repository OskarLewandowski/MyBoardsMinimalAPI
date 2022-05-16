﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Entities
{
    public class WorkItemTag
    {
        //relations many-to-many  WorkItem--[*]----[*]--Tag
        public WorkItem WorkItem { get; set; }
        public int WorkItemId { get; set; }
        public Tag Tag { get; set; }
        public int TagId { get; set; }
    }
}
