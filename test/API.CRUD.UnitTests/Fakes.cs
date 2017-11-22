using System.Collections.Generic;
using System.Linq;
using API.CRUD.Models;

namespace API.CRUD.UnitTests
{
    public static class Fakes
    {
        public static IEnumerable<Person> FakePeople => GenFu.GenFu.ListOf<Person>()
                                                             .AsEnumerable();
    }
}