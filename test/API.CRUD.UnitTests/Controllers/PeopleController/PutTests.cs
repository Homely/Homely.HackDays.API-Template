using System.Linq;
using System.Threading.Tasks;
using API.CRUD.Repositories;
using API.CRUD.UnitTests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace API.CRUD.UnitTests.Controllers.PeopleController
{
    public class PutTests
    {
        [Fact]
        public async Task GivenAModel_Put_UpdatesAndReturnsOkResult()
        {
            // Arrange
            var fakePerson = Fakes.FakePeople.First();
            var logger = new Mock<ILogger<CRUD.Controllers.PeopleController>>();
            var personRepository = new Mock<IPersonRepository>();
            personRepository.Setup(x => x.UpdateAsync(fakePerson))
                            .Returns(Task.CompletedTask);
            var controller = new CRUD.Controllers.PeopleController(logger.Object, personRepository.Object);

            // Act
            var result = await controller.Put(fakePerson);

            // Assert
            personRepository.VerifyAll();
            result.ShouldBeOk();
        }
    }
}