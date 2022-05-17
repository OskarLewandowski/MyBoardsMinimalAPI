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
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        //relations one-to-many  WorkItem--[1]----[*]--Comment
        public WorkItem WorkItem { get; set; }
        public int WorkItemId { get; set; }

        //relations one-to-many  User--[1]----[*]--Comment
        public User Author { get; set; }
        public Guid AuthorId { get; set; }
    }
}
