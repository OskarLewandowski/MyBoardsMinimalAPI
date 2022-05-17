using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string Email { get; set; }

        //relations one-to-one  User--[1]----[1]--Address
        public Address Address { get; set; }

        //relations many-to-one  WorkItem--[*]----[1]--User
        public List<WorkItem> WorkItems { get; set; } = new List<WorkItem>();

        //relations one-to-many  User--[1]----[*]--Comment
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
