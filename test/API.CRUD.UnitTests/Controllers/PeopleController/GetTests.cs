using System.Linq;
using System.Threading.Tasks;
using API.CRUD.Models;
using API.CRUD.Repositories;
using API.CRUD.UnitTests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace API.CRUD.UnitTests.Controllers.PeopleController
{
    public class GetTests
    {
        [Fact]
        public async Task GivenAPersonThatDoesntExist_Get_ReturnsNotFoundResult()
        {
            // Arrange
            const int personId = 100;
            var logger = new Mock<ILogger<CRUD.Controllers.PeopleController>>();
            var personRepository = new Mock<IPersonRepository>();
            personRepository.Setup(x => x.GetAsync(personId))
                            .ReturnsAsync((Person) null);
            var controller = new CRUD.Controllers.PeopleController(logger.Object, personRepository.Object);

            // Act
            var result = await controller.Get(personId);

            // Assert
            personRepository.VerifyAll();
            result.ShouldBeNotFound();
        }

        [Fact]
        public async Task GivenAPersonThatExists_Get_ReturnsOkResultWithPerson()
        {
            // Arrange
            var fakePerson = Fakes.FakePeople.First();
            var logger = new Mock<ILogger<CRUD.Controllers.PeopleController>>();
            var personRepository = new Mock<IPersonRepository>();
            personRepository.Setup(x => x.GetAsync(fakePerson.Id))
                            .ReturnsAsync(fakePerson);
            var controller = new CRUD.Controllers.PeopleController(logger.Object, personRepository.Object);

            // Act
            var result = await controller.Get(fakePerson.Id);

            // Assert
            personRepository.VerifyAll();
            result.ShouldBeOkWithModel(fakePerson);
        }
    }
}
