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
    public class Epic : WorkItem
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class Issue : WorkItem
    {
        public decimal Efford { get; set; }
    }

    public class Task : WorkItem
    {
        public string Activity { get; set; }
        public decimal RemaningWork { get; set; }
    }

    public abstract class WorkItem
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string IterationPath { get; set; }
        public int Priority { get; set; }

        //relations one-to-many  WorkItem--[1]----[*]--Comment
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();

        //relations many-to-one  WorkItem--[*]----[1]--User
        public virtual User Author { get; set; }
        public Guid AuthorId { get; set; }

        //relations many-to-many  WorkItem--[*]----[*]--Tag
        //public List<WorkItemTag> WorkItemTags { get; set; } = new List<WorkItemTag>();
        public virtual List<Tag> Tags { get; set; }

        //relations many-to-one  WorkItem--[*]----[1]--WorkItemState
        public virtual WorkItemState State { get; set; }
        public int StateId { get; set; }

    }
}
