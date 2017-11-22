using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.CRUD.Models;
using API.CRUD.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.CRUD.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : ApiController
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly IPersonRepository _repository;

        public PeopleController(ILogger<PeopleController> logger,
                                IPersonRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Person>), 200)]
        public async Task<IActionResult> Index()
        {
            _logger.LogDebug(nameof(Index));
            var people = await _repository.GetAsync()
                                          .ConfigureAwait(false);
            return Ok(people);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogDebug("Get: {id}", id);
            var person = await _repository.GetAsync(id)
                                          .ConfigureAwait(false);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Person), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] Person person)
        {
            _logger.LogDebug("Post: {value}", person);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personId = await _repository.AddAsync(person)
                                            .ConfigureAwait(false);
            return Created($"/api/people/{personId}", null);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Put([FromBody] Person person)
        {
            _logger.LogDebug("Put: {value}", person);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.UpdateAsync(person)
                             .ConfigureAwait(false);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogDebug("Delete: {value}", id);
            await _repository.DeleteAsync(id)
                             .ConfigureAwait(false);
            return NoContent();
        }
    }
}