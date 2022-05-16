using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        //relations one-to-many  WorkItem--[1]----[*]--Comment
        public WorkItem WorkItem { get; set; }
        public int WorkItemId { get; set; }
    }
}
