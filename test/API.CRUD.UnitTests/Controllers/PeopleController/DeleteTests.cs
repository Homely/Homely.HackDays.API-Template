using System.Linq;
using System.Threading.Tasks;
using API.CRUD.Repositories;
using API.CRUD.UnitTests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace API.CRUD.UnitTests.Controllers.PeopleController
{
    public class DeleteTests
    {
        [Fact]
        public async Task GivenAnId_Delete_DeletesAndReturnsNoContentResult()
        {
            // Arrange
            var fakePerson = Fakes.FakePeople.First();
            var logger = new Mock<ILogger<CRUD.Controllers.PeopleController>>();
            var personRepository = new Mock<IPersonRepository>();
            personRepository.Setup(x => x.DeleteAsync(fakePerson.Id))
                            .Returns(Task.CompletedTask);
            var controller = new CRUD.Controllers.PeopleController(logger.Object, personRepository.Object);

            // Act
            var result = await controller.Delete(fakePerson.Id);

            // Assert
            personRepository.VerifyAll();
            result.ShouldBeNoContent();
        }
    }
}