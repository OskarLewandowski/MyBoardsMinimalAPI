using Microsoft.EntityFrameworkCore;
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
        public string PostalCode { get; set; }
        public Coordinate Coordinate { get; set; }

        //relations one-to-one  User--[1]----[1]--Address
        public virtual User User { get; set; }
        public Guid UserId { get; set; }
    }

    //Owned types example 1 of 2 by attributes [Owned]
    //[Owned]
    public class Coordinate
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }


}
