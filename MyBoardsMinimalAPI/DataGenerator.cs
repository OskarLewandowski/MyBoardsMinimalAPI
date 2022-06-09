using Bogus;
using MyBoardsMinimalAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI
{
    public class DataGenerator
    {
        public static void Seed(MyBoardsMinimalAPIContext context)
        {
            //data language
            var locale = "pl";

            //specifying a seed to generate the same data
            Randomizer.Seed = new Random(123);

            var addressGenerator = new Faker<Address>(locale)
                //.StrictMode(true)
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.Country, f => f.Address.Country())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                .RuleFor(a => a.Street, f => f.Address.StreetName());

            var userGenerator = new Faker<User>(locale)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.Address, f => addressGenerator.Generate());

            var users = userGenerator.Generate(10);

            context.AddRange(users);
            context.SaveChanges();
        }
    }
}
