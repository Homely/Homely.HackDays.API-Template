using API.CRUD.Models;
using FluentValidation;

namespace API.CRUD.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(model => model.FirstName)
                .NotEmpty()
                .Length(1, 50);

            RuleFor(model => model.LastName)
                .NotEmpty()
                .Length(1, 50);

            RuleFor(model => model.Title)
                .NotEmpty()
                .MaximumLength(5);

            RuleFor(model => model.Age).InclusiveBetween(1, 150);
        }
    }
}