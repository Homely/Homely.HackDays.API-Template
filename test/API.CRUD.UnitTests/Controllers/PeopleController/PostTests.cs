﻿using System.Linq;
using System.Threading.Tasks;
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
        public async Task GivenAModel_Post_AddsAndReturnsCreatedResultWithLocationHeader()
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
