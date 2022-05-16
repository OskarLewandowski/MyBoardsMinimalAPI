using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostaCode { get; set; }

        //relations one-to-one
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
