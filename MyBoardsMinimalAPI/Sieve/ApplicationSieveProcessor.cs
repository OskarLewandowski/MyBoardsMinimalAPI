using Microsoft.Extensions.Options;
using MyBoardsMinimalAPI.Entities;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(IOptions<SieveOptions> options) : base(options)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<Epic>(x => x.Priority)
                .CanSort()
                .CanFilter();

            mapper.Property<Epic>(x => x.Area)
                .CanSort()
                .CanFilter();

            mapper.Property<Epic>(x => x.StartDate)
                .CanSort()
                .CanFilter();

            //to filter on client side we need to use "Author.FullName" this is be "authorFullName"
            //but we can chage this using .HasName("authorFullName")
            mapper.Property<Epic>(x => x.Author.FullName)
                .CanSort()
                .CanFilter()
                .HasName("authorFullName");

            return mapper;
        }
    }
}
