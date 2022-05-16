using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Entities
{
    public class WorkItem
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string IterationPath { get; set; }
        public int Priority { get; set; }
        //Epic
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //Issue
        public decimal Efford { get; set; }
        //Task
        public string Activity { get; set; }
        public decimal RemaningWork { get; set; }

        public string Type { get; set; }

        //relations one-to-many  WorkItem--[1]----[*]--Comment
        public List<Comment> Comments { get; set; } = new List<Comment>();

        //relations many-to-one  WorkItem--[*]----[1]--User
        public User Author { get; set; }
        public Guid AuthorId { get; set; }

        //relations many-to-many  WorkItem--[*]----[*]--Tag
        //public List<WorkItemTag> WorkItemTags { get; set; } = new List<WorkItemTag>();
        public List<Tag> Tags { get; set; }

        //relations many-to-one  WorkItem--[*]----[1]--WorkItemState
        public WorkItemState State { get; set; }
        public int StateId { get; set; }

    }
}
