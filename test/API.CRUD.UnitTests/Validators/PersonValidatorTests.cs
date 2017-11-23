using System.Collections.Generic;
using API.CRUD.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace API.CRUD.UnitTests.Validators
{
    public class PersonValidatorTests
    {
        private readonly PersonValidator _validator = new PersonValidator();

        [Fact]
        public void GivenAValidFirstName_ShouldNotHaveValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(model => model.FirstName, "FirstName");
        }

        public static IEnumerable<object[]> InvalidNames => new[]
        {
            new object[]
            {
                null
            },
            new object[]
            {
                ""
            },
            new object[]
            {
                " "
            },
            new object[]
            {
                new string('*', 51)
            }
        };

        [Theory]
        [MemberData(nameof(InvalidNames))]
        public void GivenAnInvalidFirstName_ShouldNotHaveValidationError(string firstName)
        {
            _validator.ShouldHaveValidationErrorFor(model => model.FirstName, firstName);
        }

        [Fact]
        public void GivenAValidLastName_ShouldNotHaveValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(model => model.LastName, "LastName");
        }

        [Theory]
        [MemberData(nameof(InvalidNames))]
        public void GivenAnInvalidLastName_ShouldNotHaveValidationError(string lastName)
        {
            _validator.ShouldHaveValidationErrorFor(model => model.LastName, lastName);
        }

        [Fact]
        public void GivenAValidTitle_ShouldNotHaveValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(model => model.Title, "Title");
        }

        public static IEnumerable<object[]> InvalidTitles => new[]
        {
            new object[]
            {
                null
            },
            new object[]
            {
                ""
            },
            new object[]
            {
                " "
            },
            new object[]
            {
                new string('*', 6)
            }
        };

        [Theory]
        [MemberData(nameof(InvalidTitles))]
        public void GivenAnInvalidTitle_ShouldNotHaveValidationError(string title)
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Title, title);
        }

        [Fact]
        public void GivenAValidAge_ShouldNotHaveValidationError()
        {
            _validator.ShouldNotHaveValidationErrorFor(model => model.Age, 10);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(151)]
        public void GivenAnInvalidAge_ShouldHaveValidationError(int age)
        {
            _validator.ShouldHaveValidationErrorFor(model => model.Age, age);
        }
    }
}