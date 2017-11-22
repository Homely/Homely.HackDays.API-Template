using System.Collections.Generic;
using System.Threading.Tasks;
using API.CRUD.Models;

namespace API.CRUD.Repositories
{
    public interface IPersonRepository
    {
        Task<int> AddAsync(Person person);
        Task<IEnumerable<Person>> GetAsync();
        Task<Person> GetAsync(int id);
        Task UpdateAsync(Person person);
        Task DeleteAsync(int id);
    }
}