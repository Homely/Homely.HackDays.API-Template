using System.Linq;
using System.Threading.Tasks;
using API.CRUD.Repositories;
using API.CRUD.UnitTests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace API.CRUD.UnitTests.Controllers.PeopleController
{
    public class IndexTests
    {
        [Fact]
        public async Task Index_ReturnsOkResultWithAListOfPeople()
        {
            // Arrange
            var fakePeople = Fakes.FakePeople.ToArray();
            var logger = new Mock<ILogger<CRUD.Controllers.PeopleController>>();
            var personRepository = new Mock<IPersonRepository>();
            personRepository.Setup(x => x.GetAsync())
                            .ReturnsAsync(fakePeople);
            var controller = new CRUD.Controllers.PeopleController(logger.Object, personRepository.Object);

            // Act
            var result = await controller.Index();

            // Assert
            personRepository.VerifyAll();
            result.ShouldBeOkWithModel(fakePeople);
        }
    }
}