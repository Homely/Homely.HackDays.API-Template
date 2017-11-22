using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.CRUD.Models;
using GenFu;
using Microsoft.Extensions.Logging;

namespace API.CRUD.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ILogger<PersonRepository> _logger;
        private readonly List<Person> _people;

        public PersonRepository(ILogger<PersonRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            A.Configure<Person>()
             .Fill(person => person.Id)
             .WithinRange(1, 100);
            _people = GenFu.GenFu.ListOf<Person>();
        }

        public Task<int> AddAsync(Person person)
        {
            _logger.LogDebug("AddAsync: {person}", person);

            _people.Add(person);

            return Task.FromResult(person.Id);
        }

        public Task<Person> GetAsync(int id)
        {
            _logger.LogDebug("GetAsync: {id}", id);

            return Task.FromResult(_people.FirstOrDefault(person => person.Id == id));
        }

        public Task<IEnumerable<Person>> GetAsync()
        {
            _logger.LogDebug("GetAsync");
            return Task.FromResult(_people.AsEnumerable());
        }

        public async Task UpdateAsync(Person person)
        {
            _logger.LogDebug("UpdateAsync: {person}", person);

            var existingPerson = await GetAsync(person.Id)
                                     .ConfigureAwait(false);

            existingPerson.Age = person.Age;
            existingPerson.FirstName = person.FirstName;
            existingPerson.LastName = person.LastName;
            existingPerson.Title = person.Title;
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogDebug("DeleteAsync: {id}", id);

            var existingPerson = await GetAsync(id)
                                     .ConfigureAwait(false);

            _people.Remove(existingPerson);
        }
    }
}