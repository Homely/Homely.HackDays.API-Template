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
    public class PostTests
    {
        [Fact]
        public async Task GivenAnInvalidModel_Post_ReturnsBadRequest()
        {
            // Arrange
            var logger = new Mock<ILogger<CRUD.Controllers.PeopleController>>();
            var personRepository = new Mock<IPersonRepository>();
            var controller = new CRUD.Controllers.PeopleController(logger.Object, personRepository.Object);
            controller.SetInvalidModelState();

            // Act
            var result = await controller.Post(null);

            // Assert
            personRepository.Verify(x => x.AddAsync(It.IsAny<Person>()), Times.Never);
            result.ShouldBeBadRequest();
        }

        [Fact]
        public async Task GivenAValidModel_Post_ReturnsCreatedResultWithLocationHeader()
        {
            // Arrange
            var fakePerson = Fakes.FakePeople.First();
            var logger = new Mock<ILogger<CRUD.Controllers.PeopleController>>();
            var personRepository = new Mock<IPersonRepository>();
            const int newPersonId = 100;
            personRepository.Setup(x => x.AddAsync(fakePerson))
                            .ReturnsAsync(newPersonId);
            var controller = new CRUD.Controllers.PeopleController(logger.Object, personRepository.Object);

            // Act
            var result = await controller.Post(fakePerson);

            // Assert
            personRepository.VerifyAll();
            result.ShouldBeCreated();
        }
    }
}
